using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSDProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;
    private readonly ICloudinaryService _cloudinaryService;

    public AnnouncementController(IAnnouncementService announcementService, ICloudinaryService cloudinaryService)
    {
        _announcementService = announcementService;
        _cloudinaryService = cloudinaryService;
    }

    // POST: api/announcement/create
    [HttpPost("create")]
    [Authorize(Roles = "SUPER_ADMIN,TEACHER")]
    public async Task<IActionResult> CreateAnnouncement([FromForm] AnnouncementRequest request)
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
                attachmentUrl = await _cloudinaryService.UploadImageAsync(request.Attachment, "csd-announcements");
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

        var announcement = await _announcementService.CreateAnnouncementAsync(request, userId, attachmentUrl);

        return CreatedAtAction(nameof(GetAnnouncementById), new { id = announcement.AnnouncementId }, new
        {
            Message = "Announcement created successfully",
            Announcement = announcement
        });
    }

    // GET: api/announcement/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAnnouncements([FromQuery] AnnouncementFilterRequest filter)
    {
        var result = await _announcementService.GetAllAnnouncementsAsync(filter);
        return Ok(result);
    }

    // GET: api/announcement/active
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveAnnouncements([FromQuery] string? targetAudience = null)
    {
        var announcements = await _announcementService.GetActiveAnnouncementsAsync(targetAudience);
        return Ok(announcements);
    }

    // GET: api/announcement/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAnnouncementById(int id)
    {
        var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
        if (announcement == null)
            return NotFound(new { Message = "Announcement not found" });

        // Increment view count
        await _announcementService.IncrementViewCountAsync(id);

        return Ok(announcement);
    }

    // PUT: api/announcement/update/{id}
    [HttpPut("update/{id:int}")]
    [Authorize(Roles = "SUPER_ADMIN,TEACHER")]
    public async Task<IActionResult> UpdateAnnouncement(int id, [FromForm] AnnouncementRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? attachmentUrl = null;

        // Handle file upload to Cloudinary
        if (request.Attachment != null)
        {
            try
            {
                attachmentUrl = await _cloudinaryService.UploadImageAsync(request.Attachment, "csd-announcements");
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

        var announcement = await _announcementService.UpdateAnnouncementAsync(id, request, attachmentUrl);
        if (announcement == null)
            return NotFound(new { Message = "Announcement not found" });

        return Ok(new
        {
            Message = "Announcement updated successfully",
            Announcement = announcement
        });
    }

    // DELETE: api/announcement/delete/{id}
    [HttpDelete("delete/{id:int}")]
    [Authorize(Roles = "SUPER_ADMIN,TEACHER")]
    public async Task<IActionResult> DeleteAnnouncement(int id)
    {
        var result = await _announcementService.DeleteAnnouncementAsync(id);
        if (!result)
            return NotFound(new { Message = "Announcement not found" });

        return Ok(new { Message = "Announcement deleted successfully" });
    }

    // GET: api/announcement/by-audience/{audience}
    [HttpGet("by-audience/{audience}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAnnouncementsByAudience(string audience)
    {
        var validAudiences = new[] { "All", "Student", "Teacher" };
        if (!validAudiences.Contains(audience, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { Message = "Invalid audience. Allowed: All, Student, Teacher" });

        var announcements = await _announcementService.GetActiveAnnouncementsAsync(audience);
        return Ok(announcements);
    }
}
