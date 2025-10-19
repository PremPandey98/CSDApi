using System.Text.Json.Serialization;

namespace CSDProject.Application.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsFirstTimeLogin { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? OtpRequired { get; set; }
}
