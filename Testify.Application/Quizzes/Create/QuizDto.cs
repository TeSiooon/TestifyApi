namespace Testify.Application.Quizzes.Create;

public class QuestionDto
{
    public string Text { get; set; } = default!;
    public List<AnswerDto> Answers { get; set; } = [];
}
