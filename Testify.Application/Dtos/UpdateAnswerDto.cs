using Testify.Application.Common;

namespace Testify.Application.Dtos;

public record UpdateAnswerDto
{
    public Guid? Id { get; set; }
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }
    public AnswerActionType AnswerActionType { get; set; }
}
