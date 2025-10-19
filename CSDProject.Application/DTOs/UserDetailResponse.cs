using CSDProject.Domain.Entities;

namespace CSDProject.Application.DTOs;

public class UserDetailResponse
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? AccountStatus { get; set; }
    public string? Role { get; set; }

    public static UserDetailResponse FromUser(User user)
    {
        return new UserDetailResponse
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            AccountStatus = user.AccountStatus,
            Role = user.Role
        };
    }
}