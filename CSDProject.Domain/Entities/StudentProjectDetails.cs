using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDProject.Domain.Entities
{
    [Table("csd_Student_ProjectDetails")]
    public class StudentProjectDetails
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        public string ProjectName { get; set; } = string.Empty;

        public string? ProjectDescription { get; set; }

        // For image storage
        public string? ProjectCoverImagePath { get; set; }

        public string? ProjectLink { get; set; }

        // Student User ID who created the project
        public int? ProjectCreatedBy { get; set; }

        // Student User ID who published the project  
        public int? PublishedBy { get; set; }

        // Faculty/Teacher User ID who guided the project
        public int? GuidedBy { get; set; }

        public DateTime? PublishedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Approval workflow fields
        [Required]
        public string ApprovalStatus { get; set; } = "Pending"; // Pending, Approved, Rejected
        
        public string? ApprovalToken { get; set; } // Unique token for email approval links
        
        public DateTime? ApprovalRequestedAt { get; set; }
        
        public DateTime? ApprovedAt { get; set; }
        
        public DateTime? RejectedAt { get; set; }
        
        public DateTime? TokenExpiresAt { get; set; } // 7 days from request
    }
}
