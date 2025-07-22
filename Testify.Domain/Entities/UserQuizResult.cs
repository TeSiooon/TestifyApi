using Testify.Common.Entities;

namespace Testify.Domain.Entities;

public class UserQuizResult : AuditableEntity
{
    private UserQuizResult() 
    {
        //For ORM
    }

    public UserQuizResult(Guid userId, Guid quizId, int score, int attempts, DateTime completedDate)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        QuizId = quizId;
        Score = score;
        Attempts = attempts;
        CompletedDate = completedDate;
    }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;

    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; } = default!;

    public int Score { get; private set; }
    public int Attempts { get; private set; }
    public DateTime CompletedDate { get; private set; }

    public static UserQuizResult Create(Guid userId, Guid quizId, int score, int attempts, DateTime completedDate)
    {
        return new UserQuizResult(userId, quizId, score, attempts, completedDate);
    }

    public void Update(int score, int attempts, DateTime completedDate)
    {
        Score = score;
        Attempts = attempts;
        CompletedDate = completedDate;
    }
}
