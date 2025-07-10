namespace Testify.Domain.Entities;

public class UserQuizAttempt
{
    private readonly List<UserAnswer> answers = new();

    private UserQuizAttempt()
    {

    }
    public UserQuizAttempt(Guid userId, Guid quizId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        QuizId = quizId;
        StartedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }

    public ICollection<UserAnswer> Answers => answers.AsReadOnly();

    public static UserQuizAttempt Create(Guid userId, Guid quizId)
    {
        return new UserQuizAttempt(userId, quizId);
    }

    public void Finish()
    {
        FinishedAt = DateTime.UtcNow;
    }

    public void AddAnswer(UserAnswer answer)
    {
        answers.Add(answer);
    }
}
