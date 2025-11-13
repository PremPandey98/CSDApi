using System.ComponentModel.DataAnnotations;

namespace CSDProject.Application.DTOs;

public class VerifyEmailRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required")]
    [Range(100000, 999999, ErrorMessage = "OTP must be 6 digits")]
    public int Otp { get; set; }
}
