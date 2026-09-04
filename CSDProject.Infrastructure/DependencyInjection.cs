using CSDProject.Application.Interfaces;
using CSDProject.Application.DTOs;
using CSDProject.Infrastructure.Data;
using CSDProject.Infrastructure.ScaffoldedModels;
using CSDProject.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CloudinaryDotNet;

namespace CSDProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddDbContext<DbAbe381CsddbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // Add Memory Cache for OTP storage
        services.AddMemoryCache();

        // Configure Cloudinary
        var cloudinarySettings = config.GetSection("CloudinarySettings").Get<CloudinarySettings>();
        if (cloudinarySettings != null)
        {
            var account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            );
            var cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true; // Use HTTPS
            services.AddSingleton(cloudinary);
        }

        // Add HTTP Client for external API calls (Brevo, etc.)
        services.AddHttpClient();

        // Register services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        
        // Register both email services so SMTP is preserved for future use
        services.AddScoped<EmailService>();
        services.AddScoped<BrevoEmailService>();

        // Dynamically select email provider (default to Brevo for Render compatibility)
        var emailProvider = config["EmailProvider"] ?? "Brevo";
        if (emailProvider.Equals("Smtp", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<IEmailService, EmailService>();
        }
        else
        {
            services.AddScoped<IEmailService, BrevoEmailService>();
        }

        services.AddScoped<INoticeService, NoticeService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<OtpService>();

        return services;
    }
}
