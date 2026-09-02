using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class FoEnquiryDetail
{
    public int EnquiryId { get; set; }

    public string? ClassMode { get; set; }

    public string? CourseName { get; set; }

    public DateOnly? EnquiryDate { get; set; }

    public string? EnquiryStatus { get; set; }

    public long? StudPhoneNumber { get; set; }

    public string? StudentMailId { get; set; }

    public string? StudentName { get; set; }

    public int? UserId { get; set; }

    public virtual FoUserAccountDetail? User { get; set; }
}
