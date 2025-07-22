using Testify.Common.Entities;

namespace Testify.Domain.Entities;

public class Question : AuditableEntity
{
private readonly List<Answer> answers = new List<Answer>();
    private Question()
    {
        // For ORM
    }

    public string Text { get; set; } = default!;

    public Question(Guid quizId, string text)
    {
        Id = Guid.NewGuid();
        QuizId = quizId;
        Text = text;
    }

    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; }

    public ICollection<Answer> Answers => answers.AsReadOnly();


    public static Question Create(Guid quizId, string text)
    {
        return new Question(quizId, text);
    }

    public void Update(string text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public void AddAnswer(Answer answer)
    {
        answers.Add(answer);
    }
    public void RemoveAnswer(Answer answer)
    {
        answers.Remove(answer);
    }
}
