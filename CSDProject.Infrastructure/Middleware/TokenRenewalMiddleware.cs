using CSDProject.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSDProject.Infrastructure.Middleware
{
    /// <summary>
    /// Middleware to automatically renew JWT tokens when they are close to expiry
    /// This implements sliding expiration - active users get fresh tokens automatically
    /// </summary>
    public class TokenRenewalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenRenewalMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, JwtHelper jwtHelper)
        {
            // Only process authenticated requests with Authorization header
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();

                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();

                    try
                    {
                        // Get renewal threshold from configuration (default: 15 minutes)
                        var thresholdMinutes = _configuration.GetValue<int>("CSDSetting:TokenRenewalThresholdMinutes", 15);

                        // Check if token needs renewal (expires within threshold)
                        if (jwtHelper.ShouldRenewToken(token, thresholdMinutes))
                        {
                            // Extract claims from old token
                            var claims = jwtHelper.GetClaimsFromToken(token);

                            // Generate new token with same claims
                            var newToken = GenerateNewToken(claims);

                            // Add new token to response header
                            context.Response.Headers.Append("X-New-Token", newToken);
                            context.Response.Headers.Append("Access-Control-Expose-Headers", "X-New-Token");
                        }
                    }
                    catch
                    {
                        // If token processing fails, just continue without renewal
                        // The authentication middleware will handle invalid tokens
                    }
                }
            }

            // Continue to next middleware
            await _next(context);
        }

        /// <summary>
        /// Generate a new JWT token with the same claims as the old one
        /// </summary>
        private string GenerateNewToken(ClaimsPrincipal claimsPrincipal)
        {
            // Extract existing claims
            var claims = claimsPrincipal.Claims.ToArray();

            // Get JWT settings from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["CSDSetting:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryHours = _configuration.GetValue<int>("CSDSetting:TokenExpiryHours", 1);

            // Create new token with extended expiry
            var token = new JwtSecurityToken(
                issuer: _configuration["CSDSetting:Issuer"],
                audience: _configuration["CSDSetting:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(expiryHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
