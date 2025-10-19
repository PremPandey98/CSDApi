using BCrypt.Net;
using CSDProject.Application.DTOs;
using CSDProject.Application.Interfaces;
using CSDProject.Domain.Entities;
using CSDProject.Infrastructure.Data;
using CSDProject.Infrastructure.ScaffoldedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSDProject.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly DbAbe381CsddbContext _otpDb;

        public AuthService(AppDbContext db, DbAbe381CsddbContext otpDb, IConfiguration config, IMemoryCache cache, IEmailService emailService)
        {
            _db = db;
            _otpDb = otpDb;
            _config = config;
            _cache = cache;
            _emailService = emailService;
        }

        // ---------------- Login ----------------
        public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return null;

            string accountStatus = user.AccountStatus?.ToLower() ?? "";
            string role = user.Role?.ToLower() ?? "";

            if (role == "super_admin")
                return await SuperAdminLogin(user, request.Password);

            return accountStatus switch
            {
                "lock" => await HandleFirstTimeLogin(user, request.Password),
                "unlock" => await HandleRegularLogin(user, request.Password),
                _ => null,
            };
        }
        private async Task<LoginResponse?> SuperAdminLogin(User user, string Password)
        {
            bool isPasswordValid = Password == user.Password;
            if (!isPasswordValid) return null;

            var otp = new Random().Next(100000, 999999);
            var expiryTime = DateTime.UtcNow.AddMinutes(10);
            var otpRecord = new CsdEmailValidation
            {
                Email = user.Email,
                Name = user.Name,
                Otp = otp,
                OtpStatus = "Unverified", 
                ExpiryTime = expiryTime
            };

            var record = await _otpDb.CsdEmailValidations
                .Where(x => x.Email == user.Email)
                .OrderByDescending(x => x.ExpiryTime)
                .FirstOrDefaultAsync();
            
            if(record != null)
            {
                record.ExpiryTime = expiryTime;
                record.Otp = otp;
                record.OtpStatus = "Unverified";
                await _otpDb.SaveChangesAsync();
            }
            else
            {
                _otpDb.CsdEmailValidations.Add(otpRecord);
                await _otpDb.SaveChangesAsync();
            }
            

            await _emailService.SendAdminLoginOtpEmailAsync(user.Email!, user.Name ?? "User", otp.ToString());

            return new LoginResponse
            {
                Name = user.Name ?? "",
                Message = "OTP sent to your email. Please verify.",
                OtpRequired = true
            };
        }

        // ---------------- First-Time Login ----------------
        private async Task<LoginResponse?> HandleFirstTimeLogin(User user, string tempPassword)
        {
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(tempPassword, user.Password);

            if (!isPasswordValid) return null;

            var token = GenerateJwtToken(user);
            return new LoginResponse
            {
                Token = token,
                Name = user.Name ?? "",
                IsFirstTimeLogin = true,
                Role = user.Role ?? "",
                Message = "Successfully verified temp password. Please create a new password."
            };
        }

        // ---------------- Regular Login ----------------
        private async Task<LoginResponse?> HandleRegularLogin(User user, string password)
        {
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid) return null;

            var token = GenerateJwtToken(user);
            return new LoginResponse
            {
                Token = token,
                Name = user.Name ?? "",
                IsFirstTimeLogin = false,
                Role = user.Role ?? "",
                Message = "Login successful."
            };
        }

        // ---------------- Generate JWT for Normal Login ----------------
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["CSDSetting:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["CSDSetting:Issuer"],
                audience: _config["CSDSetting:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ---------------- Generate OTP JWT ----------------
        private string GenerateOtpJwtToken(string email, string otp)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("otp", otp)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["CSDSetting:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["CSDSetting:Issuer"],
                audience: _config["CSDSetting:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ---------------- Verify OTP ----------------
        public async Task<LoginResponse?> VerifyLoginOtpAsync(string email, int enteredOtp)
        {
            var record = await _otpDb.CsdEmailValidations
                .Where(x => x.Email == email && x.Otp == enteredOtp && x.OtpStatus == "Unverified")
                .OrderByDescending(x => x.ExpiryTime)
                .FirstOrDefaultAsync();

            if (record == null || record.ExpiryTime < DateTime.UtcNow)
                return null;

            // Mark OTP as used
            record.OtpStatus = "Verified";
            await _otpDb.SaveChangesAsync();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                Name = user.Name ?? "",
                Role = user.Role ?? "",
                Message = "OTP verified, login successful.",
                OtpRequired = false
            };
        }

        // ---------------- Blacklist Token ----------------
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var blacklistedToken = await _db.BlacklistedTokens
                .FirstOrDefaultAsync(bt => bt.Token == token);

            return blacklistedToken != null;
        }

        public async Task<int> DeleteExpiredBlacklistedTokensAsync()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-7);
            var expiredTokens = _db.BlacklistedTokens.Where(t => t.Expiration < cutoffDate);
            int count = expiredTokens.Count();
            _db.BlacklistedTokens.RemoveRange(expiredTokens);
            await _db.SaveChangesAsync();
            return count;
        }

        // ---------------- Forgot Password Flow ----------------
        public async Task<bool> SendForgotPasswordOtpAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var otpCode = new Random().Next(100000, 999999).ToString();
            var cacheKey = $"forgot_pwd_otp_{email}";
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                Priority = CacheItemPriority.High
            };
            _cache.Set(cacheKey, otpCode, cacheOptions);

            await _emailService.SendOtpEmailAsync(email, user.Name ?? "User", otpCode);
            return true;
        }

        public async Task<bool> ResetPasswordWithOtpAsync(string email, string otpCode, string newPassword)
        {
            var cacheKey = $"forgot_pwd_otp_{email}";
            if (_cache.TryGetValue(cacheKey, out string? cachedOtp) && cachedOtp == otpCode)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null) return false;

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _db.SaveChangesAsync();
                _cache.Remove(cacheKey);
                return true;
            }

            return false;
        }
    }
}
