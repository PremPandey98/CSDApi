using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdTeacherTimetableDetail
{
    public int TimeTableId { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public int? CurrentYear { get; set; }

    public int? Semester { get; set; }

    public string? Title { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public virtual ICollection<CsdTeacherTimetableEntry> CsdTeacherTimetableEntries { get; set; } = new List<CsdTeacherTimetableEntry>();
}
