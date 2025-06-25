namespace Testify.Domain.Entities;

public class Question
{
    private Question()
    {
        // For ORM
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
    public int Order { get; set; } //todo moze sie przydac do wyswietlania w roznej kolejnosci
    public int MyProperty { get; set; }

    public Question(Guid quizId, string text, int order)
    {
        Id = Guid.NewGuid();
        QuizId = quizId;
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Order = order;
    }

    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; }

    public ICollection<Answer> Answers { get; private set; } = new List<Answer>();


    public static Question Create(Guid quizId, string text, int order)
    {
        return new Question(quizId, text, order);
    }

    public void Update(string text, int order)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Order = order;
    }
}
