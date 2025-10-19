namespace CSDProject.Application.DTOs;

public class UpdateAccountStatusRequest
{
    public int UserId { get; set; }
    public string NewStatus { get; set; } = string.Empty;
}
