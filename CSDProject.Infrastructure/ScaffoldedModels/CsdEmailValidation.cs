using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdEmailValidation
{
    public int EmailId { get; set; }

    public string? Email { get; set; }

    public DateTime? ExpiryTime { get; set; }

    public string? Name { get; set; }

    public int? Otp { get; set; }

    public string? OtpStatus { get; set; }
}
