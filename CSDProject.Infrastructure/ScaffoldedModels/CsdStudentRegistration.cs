using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdStudentRegistration
{
    public int StdId { get; set; }

    public int? AcademicYear { get; set; }

    public DateOnly? AdmissionDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public int? CurrentSemester { get; set; }

    public int? RollNumber { get; set; }

    public int? UnivRollNumber { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public int CourseId { get; set; }

    public int? UserId { get; set; }

    public virtual CsdCourseDetail Course { get; set; } = null!;

    public virtual CsdUserRegistration? User { get; set; }
}
