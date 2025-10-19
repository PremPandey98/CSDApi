using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdFacultyRegistration
{
    public int FacultyId { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public DateOnly? DateOfJoining { get; set; }

    public string? Designation { get; set; }

    public string? Qualification { get; set; }

    public string? Specialisation { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<CsdFacultyExperience> CsdFacultyExperiences { get; set; } = new List<CsdFacultyExperience>();

    public virtual CsdUserRegistration? User { get; set; }
}
