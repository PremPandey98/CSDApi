using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdCourseDetail
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? Description { get; set; }

    public string? Level { get; set; }

    public virtual ICollection<CsdStudentRegistration> CsdStudentRegistrations { get; set; } = new List<CsdStudentRegistration>();

    public virtual ICollection<CsdSubjectDetail> CsdSubjectDetails { get; set; } = new List<CsdSubjectDetail>();
}
