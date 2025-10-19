using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using CSDProject.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSDProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly JwtHelper _jwtHelper;

    public UserController(IUserService userService, IAuthService authService, JwtHelper jwtHelper)
    {
        _userService = userService;
        _authService = authService;
        _jwtHelper = jwtHelper;
    }

    [Authorize]
    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
    {
        // Extract token from Authorization header
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        if (string.IsNullOrEmpty(token))
            return Unauthorized("Token is required");

        // Check if token format is valid
        if (!_jwtHelper.IsValidTokenFormat(token))
            return Unauthorized("Invalid token format");

        // Check if token is expired
        if (_jwtHelper.IsTokenExpired(token))
            return Unauthorized("Token has expired");

        // Note: Blacklisted token check is now handled by BlacklistedTokenMiddleware

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized("Invalid token");

        var success = await _userService.UpdatePasswordAsync(userId, request.NewPassword);
        return success ? Ok("Password updated successfully") : NotFound("User not found");
    }

    [HttpPost("update-account-status")]
    public async Task<IActionResult> UpdateAccountStatus(UpdateAccountStatusRequest request)
    {
        if (request.UserId <= 0 || string.IsNullOrWhiteSpace(request.NewStatus))
            return BadRequest("UserId and NewStatus are required");
        var success = await _userService.UpdateAccountStatusAsync(request.UserId, request.NewStatus);
        return success ? Ok("Account status updated successfully") : NotFound("User not found");
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound("User not found");
        // Use DTO from Application.DTOs
        var userDetail = UserDetailResponse.FromUser(user);
        return Ok(userDetail);
    }

    [Authorize]
    [HttpGet("me/id")]
    public async Task<IActionResult> GetCurrentUserDetails()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized("Invalid token");

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        var userDetail = UserDetailResponse.FromUser(user);
        return Ok(userDetail);
    }
}