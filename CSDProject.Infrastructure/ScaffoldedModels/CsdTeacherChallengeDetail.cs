using System;
using System.Collections.Generic;

namespace CSDProject.Infrastructure.ScaffoldedModels;

public partial class CsdTeacherChallengeDetail
{
    public int ChallengeId { get; set; }

    public string? ChallengeName { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public DateOnly? DueDate { get; set; }

    public int? ExamTimeduration { get; set; }

    public string? GivenBy { get; set; }

    public int? Semester { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<CsdStudentChallengeResponse> CsdStudentChallengeResponses { get; set; } = new List<CsdStudentChallengeResponse>();

    public virtual ICollection<CsdTeacherChallengePoint> CsdTeacherChallengePoints { get; set; } = new List<CsdTeacherChallengePoint>();

    public virtual ICollection<CsdTeacherChallengeQuestion> CsdTeacherChallengeQuestions { get; set; } = new List<CsdTeacherChallengeQuestion>();
}
