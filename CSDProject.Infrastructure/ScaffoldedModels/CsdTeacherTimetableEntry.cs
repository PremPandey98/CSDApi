using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdTeacherTimetableEntry
{
    public int EntryId { get; set; }

    public string? ClassType { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public short? DayOfWeek { get; set; }

    public int? RoomNumber { get; set; }

    public string? SubjectName { get; set; }

    public string? TimeSlot { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public int? TimeTableId { get; set; }

    public virtual CsdTeacherTimetableDetail? TimeTable { get; set; }
}
