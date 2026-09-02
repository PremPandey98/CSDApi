using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdTeacherChallengePoint
{
    public int PointId { get; set; }

    public int? Point { get; set; }

    public int? StdId { get; set; }

    public int? ChallengeId { get; set; }

    public virtual CsdTeacherChallengeDetail? Challenge { get; set; }
}
