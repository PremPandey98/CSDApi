using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdAlumniDetail
{
    public int AluminiId { get; set; }

    public string? Designation { get; set; }

    public string? Email { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? Name { get; set; }

    public string? Oraganization { get; set; }

    public int? PassoutYear { get; set; }

    public string? Specialization { get; set; }
}
