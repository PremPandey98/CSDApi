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

            // Check if user account is deleted
            if (user.IsDeleted == true)
            {
                return new LoginResponse
                {
                    Message = "This account is not available. Please contact the administrator.",
                    OtpRequired = false
                };
            }

            // Device validation for mobile login
            if (request.IsMobileDeviceLogin)
            {
                // Validate DeviceId is provided
                if (string.IsNullOrWhiteSpace(request.DeviceId))
                {
                    return new LoginResponse
                    {
                        Message = "Device ID is required for mobile device login.",
                        OtpRequired = false
                    };
                }

                // Check if user has a stored device ID
                if (!string.IsNullOrWhiteSpace(user.DeviceId))
                {
                    // Compare device IDs
                    if (user.DeviceId != request.DeviceId)
                    {
                        // Different device - send notification to admin and block login
                        await SendDeviceMismatchNotificationAsync(user, request.DeviceId);
                        
                        return new LoginResponse
                        {
                            Message = "You are trying to log in from a different device. Please contact the administrator for assistance.",
                            OtpRequired = false
                        };
                    }
                }
            }

            string accountStatus = user.AccountStatus?.ToLower() ?? "";
            string role = user.Role?.ToLower() ?? "";

            if (role == "super_admin")
                return await SuperAdminLogin(user, request.Password);

            return accountStatus switch
            {
                "lock" => await HandleFirstTimeLogin(user, request.Password, request),
                "unlock" => await HandleRegularLogin(user, request.Password, request),
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

            var token = GenerateJwtToken(user);
            return new LoginResponse
            {
                Token = token,
                Name = user.Name ?? "",
                Role = user.Role ?? "",
                Message = "Login successful (OTP temporarily disabled).",
                OtpRequired = false
            };
        }

        // ---------------- First-Time Login ----------------
        private async Task<LoginResponse?> HandleFirstTimeLogin(User user, string tempPassword, LoginRequest request)
        {
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(tempPassword, user.Password);

            if (!isPasswordValid) return null;

            // Save device info if mobile login
            if (request.IsMobileDeviceLogin && !string.IsNullOrWhiteSpace(request.DeviceId))
            {
                user.DeviceId = request.DeviceId;
                // Note: IsMobileDeviceLogin field removed from DB - we check via DeviceId presence
                await _db.SaveChangesAsync();
            }

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
        private async Task<LoginResponse?> HandleRegularLogin(User user, string password, LoginRequest request)
        {
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid) return null;

            // Save device info if mobile login and no device stored yet
            if (request.IsMobileDeviceLogin && !string.IsNullOrWhiteSpace(request.DeviceId) && string.IsNullOrWhiteSpace(user.DeviceId))
            {
                user.DeviceId = request.DeviceId;
                // Note: IsMobileDeviceLogin field removed from DB - we check via DeviceId presence
                await _db.SaveChangesAsync();
            }

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
                expires: DateTime.UtcNow.AddHours(5),
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

        // ---------------- Email Verification Flow ----------------
        public async Task<bool> SendEmailVerificationAsync(string email, string name)
        {
            // Generate 6-digit OTP
            var otp = new Random().Next(100000, 999999);
            var expiryTime = DateTime.UtcNow.AddMinutes(10);

            // Check if there's an existing record for this email
            var existingRecord = await _otpDb.CsdEmailValidations
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.ExpiryTime)
                .FirstOrDefaultAsync();

            if (existingRecord != null)
            {
                // Update existing record
                existingRecord.Name = name;
                existingRecord.Otp = otp;
                existingRecord.OtpStatus = "Unverified";
                existingRecord.ExpiryTime = expiryTime;
            }
            else
            {
                // Generate next EmailId from the sequence or max ID
                var maxId = await _otpDb.CsdEmailValidations
                    .MaxAsync(x => (int?)x.EmailId) ?? 0;

                // Create new record with explicit EmailId
                var newRecord = new CsdEmailValidation
                {
                    EmailId = maxId + 1,  // Explicit ID assignment
                    Email = email,
                    Name = name,
                    Otp = otp,
                    OtpStatus = "Unverified",
                    ExpiryTime = expiryTime
                };
                _otpDb.CsdEmailValidations.Add(newRecord);
            }

            await _otpDb.SaveChangesAsync();

            // Send email with OTP
            await _emailService.SendOtpEmailAsync(email, name, otp.ToString());

            return true;
        }

        public async Task<bool> VerifyEmailAsync(string email, int otp)
        {
            // Find the latest unverified OTP record for this email
            var record = await _otpDb.CsdEmailValidations
                .Where(x => x.Email == email && x.Otp == otp && x.OtpStatus == "Unverified")
                .OrderByDescending(x => x.ExpiryTime)
                .FirstOrDefaultAsync();

            // Check if record exists and is not expired
            if (record == null || record.ExpiryTime < DateTime.UtcNow)
                return false;

            // Mark OTP as verified
            record.OtpStatus = "Verified";
            await _otpDb.SaveChangesAsync();

            return true;
        }

        // ---------------- Device Mismatch Notification ----------------
        private async Task SendDeviceMismatchNotificationAsync(User user, string attemptedDeviceId)
        {
            try
            {
                // Get all admin users
                var adminUsers = await _db.Users
                    .Where(u => u.Role != null && (u.Role.ToLower() == "admin" || u.Role.ToLower() == "super_admin"))
                    .Select(u => u.Email)
                    .Where(e => e != null)
                    .ToListAsync();

                if (adminUsers.Any())
                {
                    var adminEmails = adminUsers.Select(e => e!).ToList();
                    
                    await _emailService.SendDeviceMismatchNotificationAsync(
                        user.Name ?? "Unknown User",
                        user.Email ?? "Unknown Email",
                        user.Role ?? "Unknown Role",
                        user.DeviceId ?? "Unknown Device",
                        attemptedDeviceId,
                        DateTime.Now,
                        adminEmails
                    );
                }
            }
            catch (Exception)
            {
                // Log error but don't throw - email failure shouldn't block the response
            }
        }
    }
}
