using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdEventPhoto
{
    public int PhotoId { get; set; }

    public string? PhotoUrl { get; set; }

    public string? UploadedBy { get; set; }

    public DateOnly? UploadedDate { get; set; }

    public int? EventId { get; set; }

    public virtual CsdEventGallery? Event { get; set; }
}
