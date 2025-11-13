using CSDProject.Application.DTOs;

namespace CSDProject.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
    Task<bool> IsTokenBlacklistedAsync(string token);
    Task<bool> SendForgotPasswordOtpAsync(string email);
    Task<bool> ResetPasswordWithOtpAsync(string email, string otpCode, string newPassword);
    Task<LoginResponse?> VerifyLoginOtpAsync(string email, int enteredOtp);
    Task<bool> SendEmailVerificationAsync(string email, string name);
    Task<bool> VerifyEmailAsync(string email, int otp);
}
