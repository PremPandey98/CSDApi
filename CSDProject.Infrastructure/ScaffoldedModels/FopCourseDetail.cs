using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class FopCourseDetail
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? Duration { get; set; }

    public DateOnly? StartDate { get; set; }
}
