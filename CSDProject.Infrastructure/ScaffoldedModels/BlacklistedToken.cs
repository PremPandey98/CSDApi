using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class BlacklistedToken
{
    public int Id { get; set; }

    public DateOnly? Expiration { get; set; }

    public string? Token { get; set; }
}
