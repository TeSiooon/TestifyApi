using Testify.Common.Entities;

namespace Testify.Domain.Entities;

public class QuizReport : AuditableEntity
{
    private QuizReport()
    {
    }
    public QuizReport(Guid quizId, Guid reportedById, string reason)
    {
        Id = Guid.NewGuid();
        QuizId = quizId;
        ReportedById = reportedById;
        Reason = reason;
    }

    public string Reason { get; set; } = default!;

    public Quiz Quiz { get; set; } 
    public Guid QuizId { get; set; }

    public User ReportedBy { get; set; }
    public Guid ReportedById { get; set; }



    public static QuizReport Create(Guid quizId, Guid reportedBy, string reason)
    {
        return new QuizReport(quizId, reportedBy, reason);
    }
}
