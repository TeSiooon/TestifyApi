namespace Testify.Application.Quizzes.Command.Create;

public class AnswerDto
{
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }
}
