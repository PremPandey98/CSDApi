using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSDProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoticeController : ControllerBase
{
    private readonly INoticeService _noticeService;
    private readonly ICloudinaryService _cloudinaryService;

    public NoticeController(INoticeService noticeService, ICloudinaryService cloudinaryService)
    {
        _noticeService = noticeService;
        _cloudinaryService = cloudinaryService;
    }

    // POST: api/notice/create
    [HttpPost("create")]
    [Authorize(Roles = "SUPER_ADMIN,TEACHER")]
    public async Task<IActionResult> CreateNotice([FromForm] NoticeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Get UserId from ClaimTypes.NameIdentifier (this is how it's stored in JWT)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
            return Unauthorized(new { Message = "Invalid user token" });

        string? attachmentUrl = null;

        // Handle file upload to Cloudinary
        if (request.Attachment != null)
        {
            try
            {
                attachmentUrl = await _cloudinaryService.UploadImageAsync(request.Attachment, "csd-notices");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "File upload failed", Error = ex.Message });
            }
        }

        var notice = await _noticeService.CreateNoticeAsync(request, userId, attachmentUrl);

        return CreatedAtAction(nameof(GetNoticeById), new { id = notice.NoticeId }, new
        {
            Message = "Notice created successfully",
            Notice = notice
        });
    }

    // GET: api/notice/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllNotices([FromQuery] NoticeFilterRequest filter)
    {
        var result = await _noticeService.GetAllNoticesAsync(filter);
        return Ok(result);
    }

    // GET: api/notice/active
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveNotices([FromQuery] string? targetAudience = null)
    {
        var notices = await _noticeService.GetActiveNoticesAsync(targetAudience);
        return Ok(notices);
    }

    // GET: api/notice/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetNoticeById(int id)
    {
        var notice = await _noticeService.GetNoticeByIdAsync(id);
        if (notice == null)
            return NotFound(new { Message = "Notice not found" });

        // Increment view count
        await _noticeService.IncrementViewCountAsync(id);

        return Ok(notice);
    }

    // PUT: api/notice/update/{id}
    [HttpPut("update/{id:int}")]
    [Authorize(Roles = "SUPER_ADMIN,TEACHER")]
    public async Task<IActionResult> UpdateNotice(int id, [FromForm] NoticeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? attachmentUrl = null;

        // Handle file upload to Cloudinary
        if (request.Attachment != null)
        {
            try
            {
                attachmentUrl = await _cloudinaryService.UploadImageAsync(request.Attachment, "csd-notices");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "File upload failed", Error = ex.Message });
            }
        }

        var notice = await _noticeService.UpdateNoticeAsync(id, request, attachmentUrl);
        if (notice == null)
            return NotFound(new { Message = "Notice not found" });

        return Ok(new
        {
            Message = "Notice updated successfully",
            Notice = notice
        });
    }

    // DELETE: api/notice/delete/{id}
    [HttpDelete("delete/{id:int}")]
    [Authorize(Roles = "SUPER_ADMIN,TEACHER")]
    public async Task<IActionResult> DeleteNotice(int id)
    {
        var result = await _noticeService.DeleteNoticeAsync(id);
        if (!result)
            return NotFound(new { Message = "Notice not found" });

        return Ok(new { Message = "Notice deleted successfully" });
    }

    // GET: api/notice/by-audience/{audience}
    [HttpGet("by-audience/{audience}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetNoticesByAudience(string audience)
    {
        var validAudiences = new[] { "All", "Student", "Teacher" };
        if (!validAudiences.Contains(audience, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { Message = "Invalid audience. Allowed: All, Student, Teacher" });

        var notices = await _noticeService.GetActiveNoticesAsync(audience);
        return Ok(notices);
    }
}
