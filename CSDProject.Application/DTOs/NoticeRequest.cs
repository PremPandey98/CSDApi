using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CSDProject.Application.DTOs;

public class NoticeRequest
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
    public string? Category { get; set; }

    [MaxLength(20, ErrorMessage = "Priority cannot exceed 20 characters")]
    public string Priority { get; set; } = "Normal"; // Low, Normal, High, Urgent

    [MaxLength(20, ErrorMessage = "Target audience cannot exceed 20 characters")]
    public string TargetAudience { get; set; } = "All"; // All, Student, Teacher

    public bool IsActive { get; set; } = true;

    public bool IsPinned { get; set; } = false;

    public DateTime? ExpiryDate { get; set; }

    public IFormFile? Attachment { get; set; }
}
