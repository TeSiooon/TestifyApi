namespace Testify.Application.Dtos;

public record AnswerDto
{
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }
}
