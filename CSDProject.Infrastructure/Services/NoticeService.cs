using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using CSDProject.Domain.Entities;
using CSDProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CSDProject.Infrastructure.Services;

public class NoticeService : INoticeService
{
    private readonly AppDbContext _db;

    public NoticeService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<NoticeResponse> CreateNoticeAsync(NoticeRequest request, int createdBy, string? attachmentPath)
    {
        var notice = new Notice
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

        _db.Notices.Add(notice);
        await _db.SaveChangesAsync();

        return await MapToResponseAsync(notice);
    }

    public async Task<NoticeResponse?> UpdateNoticeAsync(int noticeId, NoticeRequest request, string? attachmentPath)
    {
        var notice = await _db.Notices.FindAsync(noticeId);
        if (notice == null || notice.IsDeleted) return null;

        notice.Title = request.Title;
        notice.Content = request.Content;
        notice.Category = request.Category;
        notice.Priority = request.Priority;
        notice.TargetAudience = request.TargetAudience;
        notice.IsActive = request.IsActive;
        notice.IsPinned = request.IsPinned;
        notice.ExpiryDate = request.ExpiryDate;
        notice.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(attachmentPath))
        {
            notice.AttachmentPath = attachmentPath;
        }

        await _db.SaveChangesAsync();

        return await MapToResponseAsync(notice);
    }

    public async Task<bool> DeleteNoticeAsync(int noticeId)
    {
        var notice = await _db.Notices.FindAsync(noticeId);
        if (notice == null) return false;

        // Soft delete
        notice.IsDeleted = true;
        notice.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<NoticeResponse?> GetNoticeByIdAsync(int noticeId)
    {
        var notice = await _db.Notices
            .Include(n => n.Creator)
            .FirstOrDefaultAsync(n => n.NoticeId == noticeId && !n.IsDeleted);

        if (notice == null) return null;

        return await MapToResponseAsync(notice);
    }

    public async Task<PaginatedResponse<NoticeResponse>> GetAllNoticesAsync(NoticeFilterRequest filter)
    {
        var query = _db.Notices
            .Include(n => n.Creator)
            .Where(n => !n.IsDeleted)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(n => n.Title.Contains(filter.Search) || n.Content.Contains(filter.Search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            query = query.Where(n => n.Category == filter.Category);
        }

        if (!string.IsNullOrWhiteSpace(filter.Priority))
        {
            query = query.Where(n => n.Priority == filter.Priority);
        }

        if (!string.IsNullOrWhiteSpace(filter.TargetAudience))
        {
            query = query.Where(n => n.TargetAudience == filter.TargetAudience || n.TargetAudience == "All");
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(n => n.IsActive == filter.IsActive.Value);
        }

        if (filter.IsPinned.HasValue)
        {
            query = query.Where(n => n.IsPinned == filter.IsPinned.Value);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(n => n.CreatedAt >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(n => n.CreatedAt <= filter.EndDate.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var skip = (filter.PageNumber - 1) * filter.PageSize;
        var notices = await query
            .OrderByDescending(n => n.IsPinned)
            .ThenByDescending(n => n.CreatedAt)
            .Skip(skip)
            .Take(filter.PageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var responses = new List<NoticeResponse>();
        foreach (var notice in notices)
        {
            responses.Add(await MapToResponseAsync(notice));
        }

        return new PaginatedResponse<NoticeResponse>
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

    public async Task<List<NoticeResponse>> GetActiveNoticesAsync(string? targetAudience = null)
    {
        var query = _db.Notices
            .Include(n => n.Creator)
            .Where(n => !n.IsDeleted && n.IsActive && 
                   (n.ExpiryDate == null || n.ExpiryDate > DateTime.UtcNow))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(targetAudience))
        {
            query = query.Where(n => n.TargetAudience == targetAudience || n.TargetAudience == "All");
        }

        var notices = await query
            .OrderByDescending(n => n.IsPinned)
            .ThenByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync();

        var responses = new List<NoticeResponse>();
        foreach (var notice in notices)
        {
            responses.Add(await MapToResponseAsync(notice));
        }

        return responses;
    }

    public async Task<bool> IncrementViewCountAsync(int noticeId)
    {
        var notice = await _db.Notices.FindAsync(noticeId);
        if (notice == null || notice.IsDeleted) return false;

        notice.ViewCount++;
        await _db.SaveChangesAsync();

        return true;
    }

    private async Task<NoticeResponse> MapToResponseAsync(Notice notice)
    {
        var creator = notice.Creator ?? await _db.Users.FindAsync(notice.CreatedBy);

        return new NoticeResponse
        {
            NoticeId = notice.NoticeId,
            Title = notice.Title,
            Content = notice.Content,
            Category = notice.Category,
            Priority = notice.Priority,
            TargetAudience = notice.TargetAudience,
            IsActive = notice.IsActive,
            IsPinned = notice.IsPinned,
            AttachmentUrl = notice.AttachmentPath,
            ViewCount = notice.ViewCount,
            ExpiryDate = notice.ExpiryDate,
            CreatedBy = notice.CreatedBy,
            CreatorName = creator?.Name,
            CreatorRole = creator?.Role,
            CreatedAt = notice.CreatedAt,
            UpdatedAt = notice.UpdatedAt
        };
    }
}
