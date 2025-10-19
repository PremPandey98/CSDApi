using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdSubjectDetail
{
    public int SubjectId { get; set; }

    public int? Semester { get; set; }

    public string? SubjectName { get; set; }

    public string? SyllabusUrl { get; set; }

    public int? CourseId { get; set; }

    public virtual CsdCourseDetail? Course { get; set; }
}
