namespace CSDProject.Application.DTOs;

public class NoticeResponse
{
    public int NoticeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string Priority { get; set; } = "Normal";
    public string TargetAudience { get; set; } = "All";
    public bool IsActive { get; set; }
    public bool IsPinned { get; set; }
    public string? AttachmentUrl { get; set; }
    public int ViewCount { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int CreatedBy { get; set; }
    public string? CreatorName { get; set; }
    public string? CreatorRole { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
