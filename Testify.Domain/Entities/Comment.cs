using Testify.Common.Entities;

namespace Testify.Domain.Entities;

public class Comment : AuditableEntity
{
    private Comment() 
    { 
        // For ORM
    }

    public Comment(Guid userId, Guid quizId, string content)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        QuizId = quizId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;

    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; } = default!;

    public string Content { get; private set; } = default!;


    public static Comment Create(Guid userId, Guid quizId, string content)
    {
        return new Comment(userId, quizId, content);
    }

    public void Update(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}
