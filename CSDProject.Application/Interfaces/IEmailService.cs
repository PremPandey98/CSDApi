namespace CSDProject.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendOtpEmailAsync(string toEmail, string userName, string otpCode);
    Task SendAdminLoginOtpEmailAsync(string email, string name, string otp);
    
    // Project approval email methods
    Task<bool> SendProjectApprovalEmailAsync(string teacherEmail, string teacherName, 
        string studentName, string projectName, string projectDescription, 
        string? projectLink, string approveUrl, string rejectUrl, DateTime expiryDate);
    
    Task<bool> SendProjectStatusNotificationEmailAsync(string studentEmail, string studentName,
        string teacherName, string projectName, string projectDescription, 
        bool isApproved, DateTime decisionDate);
}