using CSDProject.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CSDProject.Infrastructure.Middleware
{
    public class BlacklistedTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public BlacklistedTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            // Only check tokens for authenticated requests
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                
                if (authHeader.StartsWith("Bearer "))
                {
                    var token = authHeader.Replace("Bearer ", "");
                    
                    // Check if token is blacklisted
                    if (await authService.IsTokenBlacklistedAsync(token))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token has been invalidated");
                        return;
                    }
                }
            }

            // Continue to next middleware
            await _next(context);
        }
    }
}