using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdStudentAttendance
{
    public int AttendanceId { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public bool? AttendanceMarked { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? QrId { get; set; }

    public int? StudentId { get; set; }
}
