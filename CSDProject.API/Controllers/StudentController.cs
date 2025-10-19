using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using CSDProject.Domain.Entities;
using CSDProject.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CSDProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;
        private readonly ICloudinaryService _cloudinaryService;

        public StudentController(AppDbContext db, IEmailService emailService, ICloudinaryService cloudinaryService)
        {
            _db = db;
            _emailService = emailService;
            _cloudinaryService = cloudinaryService;
        }

        // POST: api/student/contact
        // Publicly accessible contact form
        [HttpPost("add-contact")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateContact([FromBody] ContactUsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contact = new CsdStudentContactUs
            {
                Name = request.Name,
                Email = request.Email,
                MblNumber = request.MblNumber,
                Subject = request.Subject,
                Message = request.Message
            };

            _db.StudentContactUs.Add(contact);
            await _db.SaveChangesAsync();

            // Map to response DTO if you created one
            var response = new ContactUsRequest
            {
                ContactId = contact.ContactId,
                Name = contact.Name,
                Email = contact.Email,
                MblNumber = contact.MblNumber,
                Subject = contact.Subject,
                Message = contact.Message,
                CreatedAt = DateTime.UtcNow
            };

            return CreatedAtAction(nameof(GetContactById), new { id = contact.ContactId }, response);
        }

        // GET: api/student/contacts
        // Protected - only for authorized users (e.g., Admin)
        [HttpGet("get-contacts")]
        [Authorize]
        public async Task<IActionResult> GetAllContacts()
        {
            var userName = User.Identity?.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "SUPER_ADMIN")
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = "You are not authorized to access this resource." });

            var items = await _db.StudentContactUs
                .OrderByDescending(c => c.ContactId)
                .ToListAsync();

            return Ok(items);
        }


        // GET: api/student/contacts/{id}
        [HttpGet("contacts/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _db.StudentContactUs.FindAsync(id);
            if (contact == null) return NotFound(new { Message = "Contact not found" });
            return Ok(contact);
        }

        // DELETE: api/student/contacts/{id}
        [HttpDelete("contacts/{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _db.StudentContactUs.FindAsync(id);
            if (contact == null) return NotFound(new { Message = "Contact not found" });

            _db.StudentContactUs.Remove(contact);
            await _db.SaveChangesAsync();

            return Ok(new { Message = "Contact deleted successfully" });
        }

        //------------------------------------------------------------------
        // Student Project Management Endpoints


        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromForm] StudentProjectRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request data");

            // Validate that GuidedBy teacher exists
            if (!request.GuidedBy.HasValue)
                return BadRequest("GuidedBy teacher is required");

            var teacher = await _db.Users.FindAsync(request.GuidedBy.Value);
            if (teacher == null || (teacher.Role?.ToLower() != "teacher" && teacher.Role?.ToLower() != "super_admin"))
                return BadRequest("Invalid teacher selected for guidance");

            // Validate that ProjectCreatedBy student exists
            if (!request.ProjectCreatedBy.HasValue)
                return BadRequest("ProjectCreatedBy student is required");

            var student = await _db.Users.FindAsync(request.ProjectCreatedBy.Value);
            if (student == null || student.Role?.ToLower() != "student")
                return BadRequest("Invalid student selected as creator");

            string? imagePath = null;

            if (request.ProjectCoverImage != null)
            {
                try
                {
                    // Upload to Cloudinary
                    imagePath = await _cloudinaryService.UploadImageAsync(
                        request.ProjectCoverImage,
                        "csd-projects" // Folder name in Cloudinary
                    );
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = $"Image upload failed: {ex.Message}" });
                }
            }

            // Generate approval token and expiry date
            var approvalToken = Guid.NewGuid().ToString();
            var tokenExpiresAt = DateTime.UtcNow.AddDays(7); // 7 days as requested

            var project = new StudentProjectDetails
            {
                ProjectName = request.ProjectName,
                ProjectDescription = request.ProjectDescription,
                ProjectLink = request.ProjectLink,
                ProjectCreatedBy = request.ProjectCreatedBy,
                PublishedBy = request.PublishedBy,
                GuidedBy = request.GuidedBy,
                PublishedOn = null, // Will be set when approved
                ModifiedOn = DateTime.UtcNow,
                ProjectCoverImagePath = imagePath,
                
                // Approval workflow fields
                ApprovalStatus = "Pending",
                ApprovalToken = approvalToken,
                ApprovalRequestedAt = DateTime.UtcNow,
                TokenExpiresAt = tokenExpiresAt
            };

            _db.StudentProjectDetails.Add(project);
            await _db.SaveChangesAsync();

            // Send approval email to the guiding teacher
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var approveUrl = $"{baseUrl}/api/student/approve-project/{approvalToken}";
                var rejectUrl = $"{baseUrl}/api/student/reject-project/{approvalToken}";

                // Add some validation before sending email
                if (string.IsNullOrEmpty(teacher.Email))
                {
                    return BadRequest("Teacher email is required for approval notification");
                }

                await _emailService.SendProjectApprovalEmailAsync(
                    teacher.Email!,
                    teacher.Name ?? "Teacher",
                    student.Name ?? "Student",
                    project.ProjectName,
                    project.ProjectDescription ?? "No description provided",
                    project.ProjectLink,
                    approveUrl,
                    rejectUrl,
                    tokenExpiresAt
                );

                return Ok(new
                {
                    Message = "Project submitted successfully and sent for approval",
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    Status = "Pending Approval",
                    ApprovalRequestSentTo = teacher.Name,
                    ExpiresAt = tokenExpiresAt
                });
            }
            catch (Exception ex)
            {
                // If email fails, still keep the project but log the error
                return Ok(new
                {
                    Message = "Project submitted successfully but email notification failed",
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    Status = "Pending Approval",
                    Warning = "Please contact your teacher manually for approval",
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace // For debugging - remove in production
                });
            }
        }

        // READ ALL
        [HttpGet("getAll-projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            // Only show approved projects
            var projects = await _db.StudentProjectDetails
                .Where(p => p.ApprovalStatus == "Approved")
                .OrderByDescending(p => p.ProjectId)
                .ToListAsync();

            var projectResponses = new List<StudentProjectResponse>();

            foreach (var p in projects)
            {
                // Get user names for display
                var createdByUser = p.ProjectCreatedBy.HasValue 
                    ? await _db.Users.FindAsync(p.ProjectCreatedBy.Value) 
                    : null;
                var publishedByUser = p.PublishedBy.HasValue 
                    ? await _db.Users.FindAsync(p.PublishedBy.Value) 
                    : null;
                var guidedByUser = p.GuidedBy.HasValue 
                    ? await _db.Users.FindAsync(p.GuidedBy.Value) 
                    : null;

                projectResponses.Add(new StudentProjectResponse
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    ProjectLink = p.ProjectLink,
                    ProjectCreatedBy = p.ProjectCreatedBy,
                    ProjectCreatedByName = createdByUser?.Name,
                    PublishedBy = p.PublishedBy,
                    PublishedByName = publishedByUser?.Name,
                    GuidedBy = p.GuidedBy,
                    GuidedByName = guidedByUser?.Name,
                    PublishedOn = p.PublishedOn,
                    ModifiedOn = p.ModifiedOn,
                    ApprovalStatus = p.ApprovalStatus,
                    ApprovalRequestedAt = p.ApprovalRequestedAt,
                    ApprovedAt = p.ApprovedAt,
                    RejectedAt = p.RejectedAt,
                    ProjectCoverImageUrl = !string.IsNullOrEmpty(p.ProjectCoverImagePath)
                        ? $"{baseUrl}{p.ProjectCoverImagePath}"
                        : null
                });
            }

            return Ok(projectResponses);
        }

        [HttpGet("getById-project/{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var p = await _db.StudentProjectDetails.FindAsync(id);

            if (p == null) return NotFound(new { Message = "Project not found" });

            // Get user names for display
            var createdByUser = p.ProjectCreatedBy.HasValue 
                ? await _db.Users.FindAsync(p.ProjectCreatedBy.Value) 
                : null;
            var publishedByUser = p.PublishedBy.HasValue 
                ? await _db.Users.FindAsync(p.PublishedBy.Value) 
                : null;
            var guidedByUser = p.GuidedBy.HasValue 
                ? await _db.Users.FindAsync(p.GuidedBy.Value) 
                : null;

            var res = new StudentProjectResponse
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                ProjectDescription = p.ProjectDescription,
                ProjectLink = p.ProjectLink,
                ProjectCreatedBy = p.ProjectCreatedBy,
                ProjectCreatedByName = createdByUser?.Name,
                PublishedBy = p.PublishedBy,
                PublishedByName = publishedByUser?.Name,
                GuidedBy = p.GuidedBy,
                GuidedByName = guidedByUser?.Name,
                PublishedOn = p.PublishedOn,
                ModifiedOn = p.ModifiedOn,
                ApprovalStatus = p.ApprovalStatus,
                ApprovalRequestedAt = p.ApprovalRequestedAt,
                ApprovedAt = p.ApprovedAt,
                RejectedAt = p.RejectedAt,
                ProjectCoverImageUrl = !string.IsNullOrEmpty(p.ProjectCoverImagePath)
                    ? $"{baseUrl}{p.ProjectCoverImagePath}"
                    : null
            };

            return Ok(res);
        }

        [HttpPut("update-project/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProject(int id, [FromForm] StudentProjectRequest request)
        {
            var project = await _db.StudentProjectDetails.FindAsync(id);
            if (project == null) return NotFound(new { Message = "Project not found" });

            project.ProjectName = request.ProjectName;
            project.ProjectDescription = request.ProjectDescription;
            project.ProjectLink = request.ProjectLink;
            project.ProjectCreatedBy = request.ProjectCreatedBy;
            project.PublishedBy = request.PublishedBy;
            project.GuidedBy = request.GuidedBy;
            project.ModifiedOn = DateTime.UtcNow;

            if (request.ProjectCoverImage != null)
            {
                try
                {
                    // Delete old image from Cloudinary if exists
                    if (!string.IsNullOrEmpty(project.ProjectCoverImagePath))
                    {
                        // Extract public ID from Cloudinary URL
                        var uri = new Uri(project.ProjectCoverImagePath);
                        var pathSegments = uri.AbsolutePath.Split('/');
                        var publicIdWithExtension = string.Join("/", pathSegments.Skip(pathSegments.Length - 2));
                        var publicId = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.'));
                        await _cloudinaryService.DeleteImageAsync(publicId);
                    }

                    // Upload new image to Cloudinary
                    project.ProjectCoverImagePath = await _cloudinaryService.UploadImageAsync(
                        request.ProjectCoverImage,
                        "csd-projects"
                    );
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = $"Image upload failed: {ex.Message}" });
                }
            }

            await _db.SaveChangesAsync();

            return Ok(new { Message = "Project updated successfully" });
        }

        [HttpDelete("delete-project/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _db.StudentProjectDetails.FindAsync(id);
            if (project == null) return NotFound(new { Message = "Project not found" });

            // Delete file from Cloudinary if exists
            if (!string.IsNullOrEmpty(project.ProjectCoverImagePath))
            {
                try
                {
                    // Extract public ID from Cloudinary URL
                    var uri = new Uri(project.ProjectCoverImagePath);
                    var pathSegments = uri.AbsolutePath.Split('/');
                    var publicIdWithExtension = string.Join("/", pathSegments.Skip(pathSegments.Length - 2));
                    var publicId = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.'));
                    await _cloudinaryService.DeleteImageAsync(publicId);
                }
                catch (Exception ex)
                {
                    // Log the error but continue with database deletion
                    Console.WriteLine($"Failed to delete image from Cloudinary: {ex.Message}");
                }
            }

            _db.StudentProjectDetails.Remove(project);
            await _db.SaveChangesAsync();

            return Ok(new { Message = "Project deleted successfully" });
        }

        //------------------------------------------------------------------
        // Project Approval Management Endpoints

        // GET: api/student/approve-project/{token}
        [HttpGet("approve-project/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveProject(string token)
        {
            var project = await _db.StudentProjectDetails
                .FirstOrDefaultAsync(p => p.ApprovalToken == token && p.ApprovalStatus == "Pending");

            if (project == null)
                return NotFound(new { Message = "Invalid or expired approval token" });

            // Check if token has expired (7 days)
            if (project.TokenExpiresAt.HasValue && project.TokenExpiresAt.Value < DateTime.UtcNow)
            {
                // Auto-reject expired projects
                _db.StudentProjectDetails.Remove(project);
                await _db.SaveChangesAsync();
                return BadRequest(new { Message = "Approval token has expired. Project has been automatically rejected." });
            }

            // Approve the project
            project.ApprovalStatus = "Approved";
            project.ApprovedAt = DateTime.UtcNow;
            project.ApprovalToken = null; // Invalidate token
            project.PublishedOn = DateTime.UtcNow; // Set as published

            await _db.SaveChangesAsync();

            // Send notification email to student about approval
            try
            {
                var student = project.ProjectCreatedBy.HasValue 
                    ? await _db.Users.FindAsync(project.ProjectCreatedBy.Value)
                    : null;
                var teacher = project.GuidedBy.HasValue 
                    ? await _db.Users.FindAsync(project.GuidedBy.Value)
                    : null;

                if (student?.Email != null && teacher?.Name != null)
                {
                    await _emailService.SendProjectStatusNotificationEmailAsync(
                        student.Email,
                        student.Name ?? "Student",
                        teacher.Name ?? "Teacher",
                        project.ProjectName,
                        project.ProjectDescription ?? "No description provided",
                        true, // isApproved
                        DateTime.UtcNow
                    );
                }
            }
            catch (Exception)
            {
                // Log email error but don't fail the approval
            }

            // Redirect to success page with project name
            var successUrl = $"/approval-pages/approval-success.html?projectName={Uri.EscapeDataString(project.ProjectName)}";
            return Redirect(successUrl);
        }

        // GET: api/student/reject-project/{token}
        [HttpGet("reject-project/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> RejectProject(string token)
        {
            var project = await _db.StudentProjectDetails
                .FirstOrDefaultAsync(p => p.ApprovalToken == token && p.ApprovalStatus == "Pending");

            if (project == null)
                return NotFound(new { Message = "Invalid or expired rejection token" });

            // Check if token has expired
            if (project.TokenExpiresAt.HasValue && project.TokenExpiresAt.Value < DateTime.UtcNow)
                return BadRequest(new { Message = "Approval token has expired." });

            var projectName = project.ProjectName;
            var projectDescription = project.ProjectDescription;

            // Send notification email to student about rejection before deleting
            try
            {
                var student = project.ProjectCreatedBy.HasValue 
                    ? await _db.Users.FindAsync(project.ProjectCreatedBy.Value)
                    : null;
                var teacher = project.GuidedBy.HasValue 
                    ? await _db.Users.FindAsync(project.GuidedBy.Value)
                    : null;

                if (student?.Email != null && teacher?.Name != null)
                {
                    await _emailService.SendProjectStatusNotificationEmailAsync(
                        student.Email,
                        student.Name ?? "Student",
                        teacher.Name ?? "Teacher",
                        projectName,
                        projectDescription ?? "No description provided",
                        false, // isApproved
                        DateTime.UtcNow
                    );
                }
            }
            catch (Exception)
            {
                // Log email error but continue with rejection
            }

            // TODO: Send notification email to student about rejection before deleting
            // await SendStudentNotificationEmail(project, false);

            // Delete the rejected project
            _db.StudentProjectDetails.Remove(project);
            await _db.SaveChangesAsync();

            // Redirect to rejection page with project name
            var rejectionUrl = $"/approval-pages/rejection-success.html?projectName={Uri.EscapeDataString(projectName)}";
            return Redirect(rejectionUrl);
        }

        // GET: api/student/pending-projects
        // For teachers to see projects pending their approval
        [HttpGet("pending-projects")]
        [Authorize]
        public async Task<IActionResult> GetPendingProjects()
        {
            var userName = User.Identity?.Name;
            // Get UserId from ClaimTypes.NameIdentifier (this is how it's stored in JWT)
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (!int.TryParse(userId, out int teacherId))
                return BadRequest(new { Message = "Invalid user ID" });

            // Get projects where this teacher is the guided by and status is pending
            var pendingProjects = await _db.StudentProjectDetails
                .Where(p => p.GuidedBy == teacherId && p.ApprovalStatus == "Pending")
                .OrderByDescending(p => p.ApprovalRequestedAt)
                .ToListAsync();

            var response = new List<object>();
            foreach (var project in pendingProjects)
            {
                var student = project.ProjectCreatedBy.HasValue 
                    ? await _db.Users.FindAsync(project.ProjectCreatedBy.Value)
                    : null;

                response.Add(new
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectDescription = project.ProjectDescription,
                    StudentName = student?.Name,
                    ApprovalRequestedAt = project.ApprovalRequestedAt,
                    TokenExpiresAt = project.TokenExpiresAt,
                    DaysRemaining = project.TokenExpiresAt.HasValue 
                        ? Math.Max(0, (project.TokenExpiresAt.Value - DateTime.UtcNow).Days)
                        : 0
                });
            }

            return Ok(response);
        }
    }
}
