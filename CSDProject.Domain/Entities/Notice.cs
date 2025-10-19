using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDProject.Domain.Entities;

[Table("csd_notices")]
public class Notice
{
    [Key]
    [Column("notice_id")]
    public int NoticeId { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("category")]
    [MaxLength(50)]
    public string? Category { get; set; }

    [Column("priority")]
    [MaxLength(20)]
    public string Priority { get; set; } = "Normal"; // Low, Normal, High, Urgent

    [Column("target_audience")]
    [MaxLength(20)]
    public string TargetAudience { get; set; } = "All"; // All, Student, Teacher

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("is_pinned")]
    public bool IsPinned { get; set; } = false;

    [Column("attachment_path")]
    [MaxLength(500)]
    public string? AttachmentPath { get; set; }

    [Column("view_count")]
    public int ViewCount { get; set; } = 0;

    [Column("expiry_date")]
    public DateTime? ExpiryDate { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    public User? Creator { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
