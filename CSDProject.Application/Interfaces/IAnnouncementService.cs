using CSDProject.Application.DTOs;
using CSDProject.Domain.Entities;

namespace CSDProject.Application.Interfaces;

public interface IAnnouncementService
{
    Task<AnnouncementResponse> CreateAnnouncementAsync(AnnouncementRequest request, int createdBy, string? attachmentPath);
    Task<AnnouncementResponse?> UpdateAnnouncementAsync(int announcementId, AnnouncementRequest request, string? attachmentPath);
    Task<bool> DeleteAnnouncementAsync(int announcementId);
    Task<AnnouncementResponse?> GetAnnouncementByIdAsync(int announcementId);
    Task<PaginatedResponse<AnnouncementResponse>> GetAllAnnouncementsAsync(AnnouncementFilterRequest filter);
    Task<List<AnnouncementResponse>> GetActiveAnnouncementsAsync(string? targetAudience = null);
    Task<bool> IncrementViewCountAsync(int announcementId);
}
