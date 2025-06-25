namespace Testify.Domain.Entities;

public class Comment
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

    public Guid Id { get; set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; }

    public string Content { get; private set; }


    public static Comment Create(Guid userId, Guid quizId, string content)
    {
        return new Comment(userId, quizId, content);
    }

    public void Update(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}
