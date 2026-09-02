using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdEventGallery
{
    public int EventId { get; set; }

    public DateOnly? Date { get; set; }

    public string? EventDescription { get; set; }

    public string? EventName { get; set; }

    public virtual ICollection<CsdEventPhoto> CsdEventPhotos { get; set; } = new List<CsdEventPhoto>();
}
