namespace CSDProject.Application.DTOs;

public class AnnouncementFilterRequest
{
    public string? Search { get; set; }
    public string? Category { get; set; }
    public string? Priority { get; set; }
    public string? TargetAudience { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPinned { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
