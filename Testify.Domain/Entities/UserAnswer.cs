using Testify.Common.Entities;

namespace Testify.Domain.Entities;

public class UserAnswer : AuditableEntity
{
    private UserAnswer()
    {
        // For ORM
    }

    public UserAnswer(Guid userQuizAttemptId, Guid questionId, Guid selectedAnswerId)
    {
        Id = Guid.NewGuid();
        UserQuizAttemptId = userQuizAttemptId;
        QuestionId = questionId;
        SelectedAnswerId = selectedAnswerId;
        CreatedAt = DateTime.UtcNow;
    }


    public Guid UserQuizAttemptId { get; set; }
    public UserQuizAttempt UserQuizAttempt { get; set; } = default!;

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = default!;

    public Guid SelectedAnswerId { get; set; }
    public Answer SelectedAnswer { get; set; } = default!;

    public static UserAnswer Create(Guid userQuizAttemptId, Guid questionId, Guid selectedAnswerId)
    {
        return new UserAnswer(userQuizAttemptId, questionId, selectedAnswerId);
    }
}
