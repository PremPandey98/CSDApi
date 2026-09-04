using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSDProject.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CSDProject.Infrastructure.Services;

public class BrevoEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<BrevoEmailService> _logger;

    public BrevoEmailService(HttpClient httpClient, IConfiguration config, ILogger<BrevoEmailService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    private string GetApiKey() =>
        _config["BrevoSettings:ApiKey"] 
        ?? Environment.GetEnvironmentVariable("BrevoSettings__ApiKey") 
        ?? string.Empty;

    private string GetSenderEmail() =>
        _config["BrevoSettings:SenderEmail"] 
        ?? _config["EmailSettings:FromEmail"] 
        ?? "bcpminnovation@gmail.com";

    private string GetSenderName() =>
        _config["BrevoSettings:SenderName"] 
        ?? "CSD Application";

    private async Task<bool> SendBrevoEmailInternalAsync(
        List<BrevoRecipient> recipients, 
        string subject, 
        string htmlContent)
    {
        try
        {
            var apiKey = GetApiKey();
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "placeholder")
            {
                _logger.LogError("Brevo API Key is missing or not configured properly.");
                return false;
            }

            var payload = new BrevoEmailPayload
            {
                Sender = new BrevoSender
                {
                    Name = GetSenderName(),
                    Email = GetSenderEmail()
                },
                To = recipients,
                Subject = subject,
                HtmlContent = htmlContent
            };

            var json = JsonSerializer.Serialize(payload);
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("api-key", apiKey);
            request.Headers.Add("accept", "application/json");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Email sent successfully via Brevo API: {Response}", responseBody);
                return true;
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to send email via Brevo API. Status: {Status}, Error: {Error}", 
                    response.StatusCode, errorBody);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email via Brevo API: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> SendOtpEmailAsync(string toEmail, string userName, string otpCode)
    {
        var subject = "Password Reset OTP - CSD Application";
        var body = $@"
            <html>
            <body>
                <h2>Hello {userName},</h2>
                <p>Your OTP for password reset verification is:</p>
                <h3 style='color: #007bff; font-size: 24px; letter-spacing: 3px;'>{otpCode}</h3>
                <p><strong>This code expires in 10 minutes.</strong></p>
                <p><em>Use this code to reset your password. If you didn't request this, please contact support immediately.</em></p>
                <br>
                <p>Best regards,<br>Computer Science Department,<br>Panchayat College, Bargarh</p>
            </body>
            </html>";

        var recipients = new List<BrevoRecipient>
        {
            new() { Email = toEmail, Name = userName }
        };

        return await SendBrevoEmailInternalAsync(recipients, subject, body);
    }

    public async Task SendAdminLoginOtpEmailAsync(string toEmail, string userName, string otpCode)
    {
        var subject = "Admin Login OTP - CSD Application";
        var body = $@"
            <html>
            <body>
                <h2>Hello {userName},</h2>
                <p>Your OTP for admin login verification is:</p>
                <h3 style='color: #007bff; font-size: 24px; letter-spacing: 3px;'>{otpCode}</h3>
                <p><strong>This code expires in 10 minutes.</strong></p>
                <p><em>Use this code to login as admin. If you didn't request this, please contact support immediately.</em></p>
                <br>
                <p>Best regards,<br>
                   Computer Science Department,<br>
                   Panchayat College, Bargarh</p>
            </body>
            </html>";

        var recipients = new List<BrevoRecipient>
        {
            new() { Email = toEmail, Name = userName }
        };

        var success = await SendBrevoEmailInternalAsync(recipients, subject, body);
        if (!success)
        {
            throw new Exception("Failed to send admin OTP email via Brevo");
        }
    }

    public async Task<bool> SendProjectApprovalEmailAsync(
        string teacherEmail, string teacherName, 
        string studentName, string projectName, string projectDescription, 
        string? projectLink, string approveUrl, string rejectUrl, DateTime expiryDate)
    {
        var subject = $"Project Approval Required: {projectName}";
        
        var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Project Approval Request</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #f8f9fa; padding: 20px; border-radius: 5px; text-align: center; margin-bottom: 20px; }}
        .project-details {{ border: 1px solid #dee2e6; padding: 20px; border-radius: 5px; margin: 20px 0; background-color: #f8f9fa; }}
        .action-buttons {{ text-align: center; margin: 30px 0; }}
        .btn {{ display: inline-block; padding: 12px 30px; margin: 10px; text-decoration: none; border-radius: 5px; font-weight: bold; font-size: 16px; color: white; }}
        .btn-approve {{ background-color: #28a745; }}
        .btn-reject {{ background-color: #dc3545; }}
        .footer {{ margin-top: 30px; padding-top: 20px; border-top: 1px solid #dee2e6; font-size: 12px; color: #6c757d; text-align: center; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>🎓 Student Project Approval Request</h1>
        <p>A new project requires your approval</p>
    </div>

    <p>Dear <strong>{teacherName}</strong>,</p>
    
    <p>Student <strong>{studentName}</strong> has submitted a new project that requires your approval as the guiding teacher.</p>

    <div class='project-details'>
        <h2>📋 Project Details</h2>
        <p><strong>Project Name:</strong> {projectName}</p>
        <p><strong>Description:</strong> {projectDescription}</p>
        {(string.IsNullOrEmpty(projectLink) ? "" : $"<p><strong>Project Link:</strong> <a href='{projectLink}' target='_blank'>{projectLink}</a></p>")}
        <p><strong>Created By:</strong> {studentName}</p>
        <p><strong>Submitted On:</strong> {DateTime.UtcNow:MMMM dd, yyyy}</p>
    </div>

    <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0;'>
        <strong>⏰ Important:</strong> This approval request will expire in 7 days ({expiryDate:MMMM dd, yyyy}). Please review and take action before the deadline.
    </div>

    <div class='action-buttons'>
        <h3>Please choose your action:</h3>
        <a href='{approveUrl}' class='btn btn-approve'>
            ✅ APPROVE PROJECT
        </a>
        <a href='{rejectUrl}' class='btn btn-reject'>
            ❌ REJECT PROJECT
        </a>
    </div>

    <div class='footer'>
        <p>This is an automated email from the CSD Project Management System.</p>
        <p>If you did not expect this email or have any questions, please contact the system administrator.</p>
        <p><strong>Note:</strong> Each action link can only be used once. After you approve or reject, the project status will be final.</p>
    </div>
</body>
</html>";

        // Try reading external template if available
        try
        {
            var currentDir = Directory.GetCurrentDirectory();
            var solutionDir = Directory.GetParent(currentDir)?.Parent?.FullName ?? currentDir;
            var templatePath = Path.Combine(solutionDir, "CSDProject.Infrastructure", "Templates", "ProjectApprovalEmailTemplate.html");
            
            if (!File.Exists(templatePath))
            {
                templatePath = Path.Combine(currentDir, "..", "CSDProject.Infrastructure", "Templates", "ProjectApprovalEmailTemplate.html");
            }
            
            if (File.Exists(templatePath))
            {
                emailBody = await File.ReadAllTextAsync(templatePath);
                emailBody = emailBody
                    .Replace("{{TeacherName}}", teacherName)
                    .Replace("{{StudentName}}", studentName)
                    .Replace("{{ProjectName}}", projectName)
                    .Replace("{{ProjectDescription}}", projectDescription ?? "No description provided")
                    .Replace("{{ProjectLink}}", projectLink ?? "No link provided")
                    .Replace("{{SubmissionDate}}", DateTime.UtcNow.ToString("MMMM dd, yyyy"))
                    .Replace("{{ExpiryDate}}", expiryDate.ToString("MMMM dd, yyyy"))
                    .Replace("{{ApproveUrl}}", approveUrl)
                    .Replace("{{RejectUrl}}", rejectUrl);
            }
        }
        catch
        {
            // Use inline template fallback
        }

        var recipients = new List<BrevoRecipient>
        {
            new() { Email = teacherEmail, Name = teacherName }
        };

        return await SendBrevoEmailInternalAsync(recipients, subject, emailBody);
    }

    public async Task<bool> SendProjectStatusNotificationEmailAsync(
        string studentEmail, string studentName, 
        string teacherName, string projectName, string projectDescription, 
        bool isApproved, DateTime decisionDate)
    {
        var status = isApproved ? "APPROVED" : "REJECTED";
        var subject = $"Project {status}: {projectName}";
        
        var statusColor = isApproved ? "#28a745" : "#dc3545";
        var headerBgColor = isApproved ? "#d4edda" : "#f8d7da";
        var headerBorderColor = isApproved ? "#c3e6cb" : "#f5c6cb";
        var statusIcon = isApproved ? "🎉" : "📝";
        var statusClass = isApproved ? "approved" : "rejected";

        var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Project Status Update</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ padding: 20px; border-radius: 5px; text-align: center; margin-bottom: 20px; background-color: {headerBgColor}; border: 1px solid {headerBorderColor}; }}
        .project-details {{ border: 1px solid #dee2e6; padding: 20px; border-radius: 5px; margin: 20px 0; background-color: #f8f9fa; }}
        .footer {{ margin-top: 30px; padding-top: 20px; border-top: 1px solid #dee2e6; font-size: 12px; color: #6c757d; text-align: center; }}
        .status-badge {{ display: inline-block; padding: 8px 16px; border-radius: 20px; font-weight: bold; font-size: 14px; background-color: {statusColor}; color: white; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>{statusIcon} Project Status Update</h1>
        <span class='status-badge'>{status}</span>
    </div>

    <p>Dear <strong>{studentName}</strong>,</p>
    
    <p>We have an update regarding your project submission.</p>

    <div class='project-details'>
        <h2>📋 Project Details</h2>
        <p><strong>Project Name:</strong> {projectName}</p>
        <p><strong>Description:</strong> {projectDescription}</p>
        <p><strong>Submitted On:</strong> {DateTime.UtcNow:MMMM dd, yyyy}</p>
        <p><strong>Status:</strong> <span class='status-badge'>{status}</span></p>
        <p><strong>Decision Date:</strong> {decisionDate:MMMM dd, yyyy}</p>
        <p><strong>Reviewed By:</strong> {teacherName}</p>
    </div>

    {(isApproved ? 
        @"<div style='background-color: #d4edda; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <h3>🎉 Congratulations!</h3>
            <p>Your project has been <strong>approved</strong> by your guiding teacher. Your project is now active and visible in the system.</p>
        </div>" :
        @"<div style='background-color: #f8d7da; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <h3>📝 Project Not Approved</h3>
            <p>Unfortunately, your project was not approved at this time. Please consider revising your project and resubmitting it with improvements.</p>
            <p>You may contact your guiding teacher for specific feedback and guidance.</p>
        </div>")}

    <div class='footer'>
        <p>This is an automated email from the CSD Project Management System.</p>
        <p>If you have any questions, please contact your guiding teacher or the system administrator.</p>
    </div>
</body>
</html>";

        try
        {
            var currentDir = Directory.GetCurrentDirectory();
            var solutionDir = Directory.GetParent(currentDir)?.Parent?.FullName ?? currentDir;
            var templatePath = Path.Combine(solutionDir, "CSDProject.Infrastructure", "Templates", "ProjectStatusNotificationTemplate.html");
            
            if (!File.Exists(templatePath))
            {
                templatePath = Path.Combine(currentDir, "..", "CSDProject.Infrastructure", "Templates", "ProjectStatusNotificationTemplate.html");
            }
            
            if (File.Exists(templatePath))
            {
                emailBody = await File.ReadAllTextAsync(templatePath);
                emailBody = emailBody
                    .Replace("{{StudentName}}", studentName)
                    .Replace("{{TeacherName}}", teacherName)
                    .Replace("{{ProjectName}}", projectName)
                    .Replace("{{ProjectDescription}}", projectDescription ?? "No description provided")
                    .Replace("{{SubmissionDate}}", DateTime.UtcNow.ToString("MMMM dd, yyyy"))
                    .Replace("{{Status}}", status)
                    .Replace("{{StatusClass}}", statusClass)
                    .Replace("{{StatusIcon}}", statusIcon)
                    .Replace("{{DecisionDate}}", decisionDate.ToString("MMMM dd, yyyy"))
                    .Replace("{{IsApproved}}", isApproved.ToString().ToLower());
            }
        }
        catch
        {
            // Use inline template fallback
        }

        var recipients = new List<BrevoRecipient>
        {
            new() { Email = studentEmail, Name = studentName }
        };

        return await SendBrevoEmailInternalAsync(recipients, subject, emailBody);
    }

    public async Task<bool> SendDeviceMismatchNotificationAsync(
        string userName, string userEmail, string userRole, 
        string registeredDeviceId, string attemptedDeviceId, 
        DateTime attemptTime, List<string> adminEmails)
    {
        var subject = "🔒 Security Alert: Unauthorized Device Login Attempt";
        
        string emailBody;
        try
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "DeviceMismatchNotification.html");
            if (File.Exists(templatePath))
            {
                emailBody = await File.ReadAllTextAsync(templatePath);
                emailBody = emailBody
                    .Replace("{{UserName}}", userName)
                    .Replace("{{UserEmail}}", userEmail)
                    .Replace("{{UserRole}}", userRole)
                    .Replace("{{RegisteredDeviceId}}", registeredDeviceId)
                    .Replace("{{AttemptedDeviceId}}", attemptedDeviceId)
                    .Replace("{{AttemptTime}}", attemptTime.ToString("MMM dd, yyyy hh:mm tt"))
                    .Replace("{{AlertTime}}", DateTime.Now.ToString("MMM dd, yyyy hh:mm tt"));
            }
            else
            {
                emailBody = $@"
                <html>
                <body>
                    <h2>Security Alert: Unauthorized Device Login Attempt</h2>
                    <p>An unauthorized device attempted to log in to user account: <strong>{userName}</strong> ({userEmail}).</p>
                    <p><strong>Role:</strong> {userRole}</p>
                    <p><strong>Registered Device ID:</strong> {registeredDeviceId}</p>
                    <p><strong>Attempted Device ID:</strong> {attemptedDeviceId}</p>
                    <p><strong>Attempt Time:</strong> {attemptTime:MMM dd, yyyy hh:mm tt}</p>
                </body>
                </html>";
            }
        }
        catch
        {
            emailBody = $@"
            <html>
            <body>
                <h2>Security Alert: Unauthorized Device Login Attempt</h2>
                <p>An unauthorized device attempted to log in to user account: <strong>{userName}</strong> ({userEmail}).</p>
                <p><strong>Role:</strong> {userRole}</p>
                <p><strong>Attempt Time:</strong> {attemptTime:MMM dd, yyyy hh:mm tt}</p>
            </body>
            </html>";
        }

        var recipients = adminEmails.Select(e => new BrevoRecipient { Email = e }).ToList();
        return await SendBrevoEmailInternalAsync(recipients, subject, emailBody);
    }
}

public class BrevoEmailPayload
{
    [JsonPropertyName("sender")]
    public BrevoSender Sender { get; set; } = new();

    [JsonPropertyName("to")]
    public List<BrevoRecipient> To { get; set; } = new();

    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    [JsonPropertyName("htmlContent")]
    public string HtmlContent { get; set; } = string.Empty;
}

public class BrevoSender
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}

public class BrevoRecipient
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
