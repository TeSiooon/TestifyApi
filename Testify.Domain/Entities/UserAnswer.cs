namespace Testify.Domain.Entities;

public class UserAnswer
{
    private UserAnswer()
    {
        
    }

    public UserAnswer(Guid userQuizAttemptId, Guid questionId, Guid selectedAnswerId)
    {
        Id = Guid.NewGuid();
        UserQuizAttemptId = userQuizAttemptId;
        QuestionId = questionId;
        SelectedAnswerId = selectedAnswerId;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserQuizAttemptId { get; set; }
    public UserQuizAttempt UserQuizAttempt { get; set; }

    public Guid QuestionId { get; set; }
    public Question Question { get; set; }

    public Guid SelectedAnswerId { get; set; }
    public Answer SelectedAnswer { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public static UserAnswer Create(Guid userQuizAttemptId, Guid questionId, Guid selectedAnswerId)
    {
        return new UserAnswer(userQuizAttemptId, questionId, selectedAnswerId);
    }
}
