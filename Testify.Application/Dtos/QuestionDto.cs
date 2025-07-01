namespace Testify.Application.Dtos;

public record QuestionDto
{
    public string Text { get; set; } = default!;
    public List<AnswerDto> Answers { get; set; } = [];
}
