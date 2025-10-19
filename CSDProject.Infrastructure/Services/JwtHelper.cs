using System;
using System.IdentityModel.Tokens.Jwt;

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
    }
}
