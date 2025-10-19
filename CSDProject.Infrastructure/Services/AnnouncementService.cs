using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using CSDProject.Domain.Entities;
using CSDProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CSDProject.Infrastructure.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly AppDbContext _db;

    public AnnouncementService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AnnouncementResponse> CreateAnnouncementAsync(AnnouncementRequest request, int createdBy, string? attachmentPath)
    {
        var announcement = new Announcement
        {
            Title = request.Title,
            Content = request.Content,
            Category = request.Category,
            Priority = request.Priority,
            TargetAudience = request.TargetAudience,
            IsActive = request.IsActive,
            IsPinned = request.IsPinned,
            ExpiryDate = request.ExpiryDate,
            AttachmentPath = attachmentPath,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };

        _db.Announcements.Add(announcement);
        await _db.SaveChangesAsync();

        return await MapToResponseAsync(announcement);
    }

    public async Task<AnnouncementResponse?> UpdateAnnouncementAsync(int announcementId, AnnouncementRequest request, string? attachmentPath)
    {
        var announcement = await _db.Announcements.FindAsync(announcementId);
        if (announcement == null || announcement.IsDeleted) return null;

        announcement.Title = request.Title;
        announcement.Content = request.Content;
        announcement.Category = request.Category;
        announcement.Priority = request.Priority;
        announcement.TargetAudience = request.TargetAudience;
        announcement.IsActive = request.IsActive;
        announcement.IsPinned = request.IsPinned;
        announcement.ExpiryDate = request.ExpiryDate;
        announcement.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(attachmentPath))
        {
            announcement.AttachmentPath = attachmentPath;
        }

        await _db.SaveChangesAsync();

        return await MapToResponseAsync(announcement);
    }

    public async Task<bool> DeleteAnnouncementAsync(int announcementId)
    {
        var announcement = await _db.Announcements.FindAsync(announcementId);
        if (announcement == null) return false;

        // Soft delete
        announcement.IsDeleted = true;
        announcement.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<AnnouncementResponse?> GetAnnouncementByIdAsync(int announcementId)
    {
        var announcement = await _db.Announcements
            .Include(a => a.Creator)
            .FirstOrDefaultAsync(a => a.AnnouncementId == announcementId && !a.IsDeleted);

        if (announcement == null) return null;

        return await MapToResponseAsync(announcement);
    }

    public async Task<PaginatedResponse<AnnouncementResponse>> GetAllAnnouncementsAsync(AnnouncementFilterRequest filter)
    {
        var query = _db.Announcements
            .Include(a => a.Creator)
            .Where(a => !a.IsDeleted)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(a => a.Title.Contains(filter.Search) || a.Content.Contains(filter.Search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            query = query.Where(a => a.Category == filter.Category);
        }

        if (!string.IsNullOrWhiteSpace(filter.Priority))
        {
            query = query.Where(a => a.Priority == filter.Priority);
        }

        if (!string.IsNullOrWhiteSpace(filter.TargetAudience))
        {
            query = query.Where(a => a.TargetAudience == filter.TargetAudience || a.TargetAudience == "All");
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(a => a.IsActive == filter.IsActive.Value);
        }

        if (filter.IsPinned.HasValue)
        {
            query = query.Where(a => a.IsPinned == filter.IsPinned.Value);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(a => a.CreatedAt >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(a => a.CreatedAt <= filter.EndDate.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var skip = (filter.PageNumber - 1) * filter.PageSize;
        var announcements = await query
            .OrderByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.CreatedAt)
            .Skip(skip)
            .Take(filter.PageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var responses = new List<AnnouncementResponse>();
        foreach (var announcement in announcements)
        {
            responses.Add(await MapToResponseAsync(announcement));
        }

        return new PaginatedResponse<AnnouncementResponse>
        {
            Data = responses,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = filter.PageNumber > 1,
            HasNextPage = filter.PageNumber < totalPages
        };
    }

    public async Task<List<AnnouncementResponse>> GetActiveAnnouncementsAsync(string? targetAudience = null)
    {
        var query = _db.Announcements
            .Include(a => a.Creator)
            .Where(a => !a.IsDeleted && a.IsActive && 
                   (a.ExpiryDate == null || a.ExpiryDate > DateTime.UtcNow))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(targetAudience))
        {
            query = query.Where(a => a.TargetAudience == targetAudience || a.TargetAudience == "All");
        }

        var announcements = await query
            .OrderByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.CreatedAt)
            .Take(50)
            .ToListAsync();

        var responses = new List<AnnouncementResponse>();
        foreach (var announcement in announcements)
        {
            responses.Add(await MapToResponseAsync(announcement));
        }

        return responses;
    }

    public async Task<bool> IncrementViewCountAsync(int announcementId)
    {
        var announcement = await _db.Announcements.FindAsync(announcementId);
        if (announcement == null || announcement.IsDeleted) return false;

        announcement.ViewCount++;
        await _db.SaveChangesAsync();

        return true;
    }

    private async Task<AnnouncementResponse> MapToResponseAsync(Announcement announcement)
    {
        var creator = announcement.Creator ?? await _db.Users.FindAsync(announcement.CreatedBy);

        return new AnnouncementResponse
        {
            AnnouncementId = announcement.AnnouncementId,
            Title = announcement.Title,
            Content = announcement.Content,
            Category = announcement.Category,
            Priority = announcement.Priority,
            TargetAudience = announcement.TargetAudience,
            IsActive = announcement.IsActive,
            IsPinned = announcement.IsPinned,
            AttachmentUrl = announcement.AttachmentPath,
            ViewCount = announcement.ViewCount,
            ExpiryDate = announcement.ExpiryDate,
            CreatedBy = announcement.CreatedBy,
            CreatorName = creator?.Name,
            CreatorRole = creator?.Role,
            CreatedAt = announcement.CreatedAt,
            UpdatedAt = announcement.UpdatedAt
        };
    }
}
