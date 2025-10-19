# CSD Project API

A comprehensive ASP.NET Core Web API for managing Computer Science Department projects, notices, announcements, and student interactions.

## 🚀 Features

### Core Modules
- **Authentication & Authorization** - JWT-based authentication with role-based access control (SUPER_ADMIN, TEACHER, STUDENT)
- **User Management** - User registration, login, password reset with OTP verification
- **Notice Management** - CRUD operations for notices with file attachments
- **Announcement Management** - CRUD operations for announcements with file attachments
- **Student Project Management** - Project submission with approval workflow
- **Contact Us** - Student contact form management

### Key Features
✅ **Clean Architecture** - Domain, Application, Infrastructure, and API layers  
✅ **Cloudinary Integration** - Cloud-based file storage for images and documents  
✅ **JWT Authentication** - Secure token-based authentication  
✅ **Role-Based Authorization** - Three user roles with different permissions  
✅ **Email Notifications** - Automated email sending via SMTP  
✅ **Pagination & Filtering** - Efficient data retrieval with search capabilities  
✅ **Soft Delete** - Safe deletion of records with recovery option  
✅ **Project Approval Workflow** - Teacher approval system for student projects  

## 🛠️ Technology Stack

- **Framework:** .NET 9.0
- **Database:** SQL Server with Entity Framework Core
- **Cloud Storage:** Cloudinary
- **Authentication:** JWT (JSON Web Tokens)
- **Email:** SMTP (Gmail)
- **Architecture:** Clean Architecture Pattern

## 📦 NuGet Packages

```xml
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Authentication.JwtBearer
- System.IdentityModel.Tokens.Jwt
- CloudinaryDotNet
- Swashbuckle.AspNetCore (Swagger)
```

## 🏗️ Project Structure

```
CSDProject/
├── CSDProject.API/              # API Controllers & Configuration
│   ├── Controllers/             # REST API Controllers
│   ├── wwwroot/                 # Static files
│   └── appsettings.json         # Configuration
├── CSDProject.Application/      # DTOs & Interfaces
│   ├── DTOs/                    # Data Transfer Objects
│   └── Interfaces/              # Service Interfaces
├── CSDProject.Domain/           # Domain Entities
│   ├── Entities/                # Database Models
│   └── Common/                  # Common utilities
└── CSDProject.Infrastructure/   # Data Access & Services
    ├── Data/                    # DbContext & Migrations
    ├── Services/                # Business Logic Services
    └── Middleware/              # Custom Middleware
```

## ⚙️ Configuration

### 1. Database Connection
Update `appsettings.json` with your SQL Server connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. JWT Settings
```json
"JwtSettings": {
  "SecretKey": "YOUR_SECRET_KEY",
  "Issuer": "CSDProject",
  "Audience": "CSDProjectUsers",
  "ExpiryMinutes": 60
}
```

### 3. Cloudinary Settings
```json
"CloudinarySettings": {
  "CloudName": "YOUR_CLOUD_NAME",
  "ApiKey": "YOUR_API_KEY",
  "ApiSecret": "YOUR_API_SECRET"
}
```

### 4. Email Settings
```json
"SmtpSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "Username": "your-email@gmail.com",
  "Password": "your-app-password",
  "FromEmail": "your-email@gmail.com",
  "FromName": "CSD Team"
}
```

## 🚦 Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- SQL Server (Express or higher)
- Visual Studio 2022 or VS Code
- Cloudinary account (free tier available)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/PremPandey98/CSDApi.git
cd CSDApi
```

2. **Restore NuGet packages**
```bash
dotnet restore
```

3. **Update appsettings.json**
   - Configure database connection string
   - Add JWT secret key
   - Add Cloudinary credentials
   - Add SMTP settings

4. **Run migrations**
```bash
cd CSDProject.Infrastructure
dotnet ef database update
```

5. **Run the application**
```bash
cd ../CSDProject.API
dotnet run
```

6. **Access Swagger UI**
```
https://localhost:7193/swagger
```

## 📚 API Endpoints

### Authentication
- `POST /api/Auth/register` - Register new user
- `POST /api/Auth/login` - User login
- `POST /api/Auth/forgot-password` - Request password reset
- `POST /api/Auth/verify-otp` - Verify OTP
- `POST /api/Auth/update-password` - Update password
- `POST /api/Auth/logout` - Logout user

### Notice Management
- `GET /api/Notice` - Get all notices (paginated)
- `GET /api/Notice/active` - Get active notices
- `GET /api/Notice/{id}` - Get notice by ID
- `POST /api/Notice` - Create notice (Admin/Teacher)
- `PUT /api/Notice/{id}` - Update notice (Admin/Teacher)
- `DELETE /api/Notice/{id}` - Delete notice (Admin/Teacher)

### Announcement Management
- `GET /api/Announcement` - Get all announcements (paginated)
- `GET /api/Announcement/active` - Get active announcements
- `GET /api/Announcement/{id}` - Get announcement by ID
- `POST /api/Announcement` - Create announcement (Admin/Teacher)
- `PUT /api/Announcement/{id}` - Update announcement (Admin/Teacher)
- `DELETE /api/Announcement/{id}` - Delete announcement (Admin/Teacher)

### Student Project Management
- `POST /api/Student/create-project` - Submit project for approval
- `GET /api/Student/projects` - Get all projects
- `GET /api/Student/pending-projects` - Get pending projects (Teacher)
- `PUT /api/Student/update-project/{id}` - Update project
- `DELETE /api/Student/delete-project/{id}` - Delete project
- `GET /api/Student/approve-project/{token}` - Approve project (via email link)
- `GET /api/Student/reject-project/{token}` - Reject project (via email link)

### User Management
- `GET /api/User` - Get all users
- `GET /api/User/{id}` - Get user by ID
- `PUT /api/User/update-account-status` - Update user account status

## 🔐 User Roles

- **SUPER_ADMIN** - Full access to all features
- **TEACHER** - Manage notices, announcements, approve projects
- **STUDENT** - Submit projects, view notices/announcements

## 📝 File Upload Support

Supported file types:
- **Images:** JPG, JPEG, PNG
- **Documents:** PDF, DOC, DOCX
- **Max Size:** 10MB per file

Files are stored in Cloudinary with automatic CDN delivery.

## 🧪 Testing

See `NoticeAnnouncementAPI.http` and `CSDProject.API.http` files for sample API requests.

## 📄 Documentation

- [Implementation Summary](IMPLEMENTATION_SUMMARY.md) - Detailed implementation guide
- [Quick Start Guide](QUICK_START_GUIDE.md) - Quick setup instructions
- [Cloudinary Migration](CLOUDINARY_MIGRATION.md) - Cloud storage setup guide

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📧 Contact

**Developer:** Prem Pandey  
**Repository:** [https://github.com/PremPandey98/CSDApi](https://github.com/PremPandey98/CSDApi)

## 📜 License

This project is licensed under the MIT License.

## 🙏 Acknowledgments

- ASP.NET Core Team
- Entity Framework Core Team
- Cloudinary Team
- JWT Authentication Community

---

**Note:** Remember to add `appsettings.Development.json` to your local environment with sensitive credentials. This file is excluded from version control for security reasons.
