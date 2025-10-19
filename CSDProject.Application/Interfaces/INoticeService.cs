using CSDProject.Application.DTOs;
using CSDProject.Domain.Entities;

namespace CSDProject.Application.Interfaces;

public interface INoticeService
{
    Task<NoticeResponse> CreateNoticeAsync(NoticeRequest request, int createdBy, string? attachmentPath);
    Task<NoticeResponse?> UpdateNoticeAsync(int noticeId, NoticeRequest request, string? attachmentPath);
    Task<bool> DeleteNoticeAsync(int noticeId);
    Task<NoticeResponse?> GetNoticeByIdAsync(int noticeId);
    Task<PaginatedResponse<NoticeResponse>> GetAllNoticesAsync(NoticeFilterRequest filter);
    Task<List<NoticeResponse>> GetActiveNoticesAsync(string? targetAudience = null);
    Task<bool> IncrementViewCountAsync(int noticeId);
}
