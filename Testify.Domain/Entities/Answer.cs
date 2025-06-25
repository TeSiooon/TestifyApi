namespace Testify.Domain.Entities;

public class Answer
{
    private Answer()
    {
        // For ORM
    }
    public Answer(Guid questionId, string text, bool isCorrect)
    {
        Id = Guid.NewGuid();
        QuestionId = questionId;
        Text = text ?? throw new ArgumentNullException(nameof(text));
        IsCorrect = isCorrect;
    }

    public Guid Id { get; set; }
    public string Text { get; private set; } = default!;
    public bool IsCorrect { get; private set; }

    public Guid? QuestionId { get; private set; }
    public Question? Question { get; private set; }

    public static Answer Create(Guid questionId, string text, bool isCorrect)
    {
        return new Answer(questionId, text, isCorrect);
    }

    public void Update(string text, bool isCorrect)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        IsCorrect = isCorrect;
    }
}
