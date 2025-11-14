using CSDProject.Infrastructure;
using CSDProject.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on the port provided by Koyeb
var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// -----------------------------
// 1️⃣ Add Services
// -----------------------------
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<JwtHelper>();

// -----------------------------
// 2️⃣ Configure JWT Authentication
// -----------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["CSDSetting:SecretKey"]!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["CSDSetting:Issuer"],
            ValidAudience = builder.Configuration["CSDSetting:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// -----------------------------
// 3️⃣ Build App
// -----------------------------
var app = builder.Build();

// -----------------------------
// 4️⃣ CORS - Allow All Origins
// -----------------------------
app.UseCors(builder =>
{
    builder
        .AllowAnyOrigin()      // Allow all websites/origins
        .AllowAnyMethod()      // Allow GET, POST, PUT, DELETE, etc.
        .AllowAnyHeader();     // Allow all headers
    // Note: .AllowCredentials() cannot be used with .AllowAnyOrigin()
});

// -----------------------------
// 5️⃣ Authentication + Middleware
// -----------------------------
app.UseAuthentication();
app.UseMiddleware<CSDProject.Infrastructure.Middleware.BlacklistedTokenMiddleware>();
app.UseAuthorization();

// -----------------------------
// 6️⃣ Static Files (VERY IMPORTANT for image access)
// -----------------------------
app.UseStaticFiles();
// This allows accessing images from wwwroot/uploads
// e.g. https://localhost:5001/uploads/myimage.jpg

// -----------------------------
// 7️⃣ Map Controllers
// -----------------------------
app.MapControllers();

// -----------------------------
// 8️⃣ Run
// -----------------------------
app.Run();
