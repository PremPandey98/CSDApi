using Microsoft.AspNetCore.Http;

public class StudentProjectRequest
{
    public string ProjectName { get; set; } = string.Empty;
    public string? ProjectDescription { get; set; }
    public IFormFile? ProjectCoverImage { get; set; } // file from form-data
    public string? ProjectLink { get; set; }
    public int? ProjectCreatedBy { get; set; } // Student user ID
    public int? PublishedBy { get; set; } // Student user ID who published
    public int? GuidedBy { get; set; } // Faculty/Teacher user ID
}
