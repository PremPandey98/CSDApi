using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using CSDProject.Domain.Entities;
using CSDProject.Infrastructure.Data;
using CSDProject.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CSDProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AppDbContext _db;
    private readonly JwtHelper _jwtHelper;

    public AuthController(IAuthService authService, AppDbContext db, JwtHelper jwtHelper)
    {
        _authService = authService;
        _db = db;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var response = await _authService.AuthenticateAsync(request);
        if (response == null) return Unauthorized("Invalid credentials or inactive account");
        return Ok(response);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || request.OtpCode <= 0)
            return BadRequest(new { Message = "Email and OTP are required." });

        // Call the updated AuthService method with email and OTP
        var result = await _authService.VerifyLoginOtpAsync(request.Email, request.OtpCode);
        if (result == null)
            return Unauthorized(new { Message = "Invalid or expired OTP" });

        return Ok(result);
    }

    [HttpPost("send-email-verification")]
    public async Task<IActionResult> SendEmailVerification([FromBody] SendEmailVerificationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _authService.SendEmailVerificationAsync(request.Email, request.Name);
            if (!result)
                return BadRequest(new { Message = "Failed to send verification email" });

            return Ok(new { Message = "Verification email sent successfully. Please check your inbox." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        }
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _authService.VerifyEmailAsync(request.Email, request.Otp);
            if (!result)
                return BadRequest(new { Message = "Invalid or expired OTP" });

            return Ok(new { Message = "Email verified successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email is required");
        var sent = await _authService.SendForgotPasswordOtpAsync(request.Email);
        if (!sent) return NotFound("User not found");
        return Ok("OTP sent to your email address");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.OtpCode) || string.IsNullOrWhiteSpace(request.NewPassword))
            return BadRequest("Email, OTP, and new password are required");
        var success = await _authService.ResetPasswordWithOtpAsync(request.Email, request.OtpCode, request.NewPassword);
        if (!success) return BadRequest("Invalid or expired OTP, or user not found");
        return Ok("Password reset successfully");
    }



    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // Get the expiry from JWT token
        var exp = _jwtHelper.GetExpiryFromJwt(token);

        var blacklistedToken = new BlacklistedToken
        {
            Token = token,
            Expiration = exp
        };

        _db.BlacklistedTokens.Add(blacklistedToken);
        await _db.SaveChangesAsync();

        return Ok("Logged out successfully");
    }
}
