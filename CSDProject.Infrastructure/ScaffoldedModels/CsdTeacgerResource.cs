using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdTeacgerResource
{
    public int ResourceId { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public string? Description { get; set; }

    public string? ResourceType { get; set; }

    public int? Semseter { get; set; }

    public string? Subject { get; set; }

    public string? Title { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public string? Url { get; set; }
}
