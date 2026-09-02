using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdTeacherAttendanceQr
{
    public int AttendanceId { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ExpireTime { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? QrCodeValue { get; set; }

    public int? Semester { get; set; }

    public string? SubjectName { get; set; }
}
