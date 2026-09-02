using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class InternalMarkDetailsEntity
{
    public int MarkId { get; set; }

    public double? MarkSecured { get; set; }

    public string? PublishedBy { get; set; }

    public DateOnly? PublishedDate { get; set; }

    public int? RollNumber { get; set; }

    public double? SecuredLabMark { get; set; }

    public double? SecuredWrittenMark { get; set; }

    public int? Semester { get; set; }

    public string? SubjectName { get; set; }

    public double? TotalExamMark { get; set; }

    public double? TotalLabMark { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }
}
