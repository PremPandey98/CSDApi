using System.ComponentModel.DataAnnotations;

namespace CSDProject.Application.DTOs;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? DeviceId { get; set; }

    public bool IsMobileDeviceLogin { get; set; }
}
