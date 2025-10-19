using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdFacultyExperience
{
    public int ExperienceId { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? InstituteName { get; set; }

    public string? Position { get; set; }

    public DateOnly? StartDate { get; set; }

    public int? FacultyId { get; set; }

    public virtual CsdFacultyRegistration? Faculty { get; set; }
}
