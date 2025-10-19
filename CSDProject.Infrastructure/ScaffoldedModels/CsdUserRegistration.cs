using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdUserRegistration
{
    public int UserId { get; set; }

    public string? AccountStatus { get; set; }

    public string? Address { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public bool? IsDeleted { get; set; }

    public long? MobileNumber { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? ProfilePhotoUrl { get; set; }

    public string? Role { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public virtual CsdAdminRegistration? CsdAdminRegistration { get; set; }

    public virtual CsdFacultyRegistration? CsdFacultyRegistration { get; set; }

    public virtual CsdStudentRegistration? CsdStudentRegistration { get; set; }
}
