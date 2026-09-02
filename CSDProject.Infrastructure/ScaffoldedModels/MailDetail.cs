using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class MailDetail
{
    public int Id { get; set; }

    public string? Content { get; set; }

    public string? DraftName { get; set; }

    public string? Name { get; set; }

    public string? Path { get; set; }

    public string? Subject { get; set; }

    public DateTime? LastBatchSentAt { get; set; }

    public int? TotalMailsSent { get; set; }

    public string? ResumePath { get; set; }

    public string? ResumeContentType { get; set; }

    public string? ResumeFileName { get; set; }
}
