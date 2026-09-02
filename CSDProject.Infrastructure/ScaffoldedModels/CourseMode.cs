using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CourseMode
{
    public int CourseId { get; set; }

    public string? Mode { get; set; }

    public virtual FoCourseDetail Course { get; set; } = null!;
}
