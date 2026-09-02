using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class FopUserAccountDetail
{
    public int UserId { get; set; }

    public string? AccountStatus { get; set; }

    public string? MailId { get; set; }

    public string? Password { get; set; }

    public long? PhoneNumber { get; set; }

    public string? UserName { get; set; }

    public virtual ICollection<FopEnquiryDetail> FopEnquiryDetails { get; set; } = new List<FopEnquiryDetail>();
}
