using CSDProject.Application.DTOs;
using CSDProject.Domain.Entities;

namespace CSDProject.Application.Interfaces;

public interface IUserService
{
    Task<bool> UpdatePasswordAsync(int userId, string newPassword);
    Task<bool> UpdateAccountStatusAsync(int userId, string newStatus);
    Task<User?> GetUserByIdAsync(int userId);
}