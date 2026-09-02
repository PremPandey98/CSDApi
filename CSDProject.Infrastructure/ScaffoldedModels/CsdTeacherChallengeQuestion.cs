using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdTeacherChallengeQuestion
{
    public int QuestionId { get; set; }

    public string? CorrectAnswer { get; set; }

    public byte[]? OptionList { get; set; }

    public string? Questions { get; set; }

    public int? ChallengeId { get; set; }

    public virtual CsdTeacherChallengeDetail? Challenge { get; set; }

    public virtual ICollection<CsdStudentChallengeResponse> CsdStudentChallengeResponses { get; set; } = new List<CsdStudentChallengeResponse>();
}
