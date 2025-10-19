public class StudentProjectResponse
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? ProjectDescription { get; set; }
    public string? ProjectLink { get; set; }
    public int? ProjectCreatedBy { get; set; } // Student user ID
    public string? ProjectCreatedByName { get; set; } // Student name for display
    public int? PublishedBy { get; set; } // Student user ID
    public string? PublishedByName { get; set; } // Student name for display
    public int? GuidedBy { get; set; } // Faculty user ID
    public string? GuidedByName { get; set; } // Faculty name for display
    public DateTime? PublishedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    // Instead of Base64 (BLOB), we now return the public image URL
    public string? ProjectCoverImageUrl { get; set; }
    
    // Approval workflow fields
    public string ApprovalStatus { get; set; } = string.Empty;
    public DateTime? ApprovalRequestedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
}
