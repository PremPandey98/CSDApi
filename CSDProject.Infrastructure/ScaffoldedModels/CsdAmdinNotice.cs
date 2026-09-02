using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdAmdinNotice
{
    public int NoticeId { get; set; }

    public string? Description { get; set; }

    public string? ModifiedBy { get; set; }

    public DateOnly? ModifiedOn { get; set; }

    public string? NoticesType { get; set; }

    public string? SentBy { get; set; }

    public DateOnly? SentOn { get; set; }

    public string? Title { get; set; }
}
