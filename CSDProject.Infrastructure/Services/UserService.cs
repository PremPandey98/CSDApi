using CSDProject.Application.Interfaces;
using CSDProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CSDProject.Domain.Entities;

namespace CSDProject.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return false;

            // Hash new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            
            // If this is first-time login (account_status = "lock"), change to "unlock"
            if (user.AccountStatus?.ToLower() == "lock")
            {
                user.AccountStatus = "unlock";
            }
            
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAccountStatusAsync(int userId, string newStatus)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return false;
            user.AccountStatus = newStatus;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}