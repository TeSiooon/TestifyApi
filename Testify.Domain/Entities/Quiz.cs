using Testify.Common.Entities;
using Testify.Domain.Constants;

namespace Testify.Domain.Entities;

public class Quiz : AuditableEntity
{
    private readonly List<Question> questions = new();
    private Quiz() 
    {

    }

    public Quiz(string title, string description, QuizCategoryType category, bool isPrivate,
        int maxAttempts, TimeSpan? timeLimit)
    {
        Id = Guid.NewGuid();
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        Category = category;
        IsPrivate = isPrivate;
        MaxAttempts = maxAttempts;
        TimeLimit = timeLimit;
    }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public QuizCategoryType Category { get; set; }
    public bool IsPrivate { get; set; }
    public int MaxAttempts { get; set; }
    public TimeSpan? TimeLimit { get; set; }

    public ICollection<Question> Questions => questions.AsReadOnly();
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    public static Quiz Create(string title, string description, QuizCategoryType category, bool isPrivate,
        int maxAttempts, TimeSpan? timeLimit)
    {
        return new Quiz(title, description, category, isPrivate, maxAttempts, timeLimit);
    }

    public void Update(string title, string description, QuizCategoryType category, bool isPrivate,
                       int maxAttempts, TimeSpan? timeLimit)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        Category = category;
        IsPrivate = isPrivate;
        MaxAttempts = maxAttempts;
        TimeLimit = timeLimit;
    }

    public void AddQuestion(Question question)
    {
        questions.Add(question);
    }
}
