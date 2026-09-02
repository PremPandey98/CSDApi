using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CSDProject.Infrastructure.Services
{
    public class JwtHelper
    {
        public DateTime GetExpiryFromJwt(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Extract the expiration date (exp claim)
                var exp = jwtToken.ValidTo;

                return exp;
            }
            catch (Exception ex)
            {
                // Handle token parsing exceptions (invalid token format)
                throw new InvalidOperationException("Invalid JWT token.", ex);
            }
        }

        public bool IsTokenExpired(string token)
        {
            try
            {
                var expiry = GetExpiryFromJwt(token);
                return expiry < DateTime.UtcNow;
            }
            catch
            {
                return true; // Consider invalid tokens as expired
            }
        }

        public bool IsValidTokenFormat(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if token is close to expiry and needs renewal
        /// </summary>
        /// <param name="token">JWT token to check</param>
        /// <param name="thresholdMinutes">Minutes before expiry to trigger renewal</param>
        /// <returns>True if token should be renewed</returns>
        public bool ShouldRenewToken(string token, int thresholdMinutes = 15)
        {
            try
            {
                var expiry = GetExpiryFromJwt(token);
                var renewalTime = DateTime.UtcNow.AddMinutes(thresholdMinutes);
                
                // If token expires within the threshold, it should be renewed
                return expiry <= renewalTime;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extract all claims from the token
        /// </summary>
        public ClaimsPrincipal GetClaimsFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid JWT token.", ex);
            }
        }
    }
}
