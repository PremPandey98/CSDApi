using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdAdminRegistration
{
    public int AdminId { get; set; }

    public string? ApproveStatus { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public DateOnly? DateOfJoining { get; set; }

    public string? Designation { get; set; }

    public string? Qualification { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public int? UserId { get; set; }

    public virtual CsdUserRegistration? User { get; set; }
}
