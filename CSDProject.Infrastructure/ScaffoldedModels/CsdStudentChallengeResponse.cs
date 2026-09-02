using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdStudentChallengeResponse
{
    public int ResponseId { get; set; }

    public bool? IsSkipped { get; set; }

    public string? SelectedOption { get; set; }

    public int? StudentId { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public int? ChallengeId { get; set; }

    public int? QuestionId { get; set; }

    public virtual CsdTeacherChallengeDetail? Challenge { get; set; }

    public virtual CsdTeacherChallengeQuestion? Question { get; set; }
}
