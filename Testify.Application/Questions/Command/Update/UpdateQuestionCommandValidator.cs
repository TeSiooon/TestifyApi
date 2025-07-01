using FluentValidation;

namespace Testify.Application.Questions.Command.Update;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("QuestionId must be provided.");

        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Question text must not be empty.")
            .MaximumLength(500)
            .WithMessage("Question text cannot exceed 500 characters.");

        RuleFor(x => x.Answers)
            .NotNull()
            .WithMessage("Answers collection must not be null.")
            .Must(ans => ans.Count >= 2)
            .WithMessage("Each question should have at least two answers.")
            .Must(ans => ans.Count <= 5)
            .WithMessage("Each question cannot have more than 5 answers.")
            .Must(ans => ans.Count(a => a.IsCorrect) == 1)
            .WithMessage("Each question must have exactly one correct answer.");

        RuleForEach(x => x.Answers).ChildRules(answer =>
        {
            answer.RuleFor(a => a.Text)
                  .NotEmpty()
                  .WithMessage("Answer text must not be empty.")
                  .MaximumLength(500)
                  .WithMessage("Answer text cannot exceed 500 characters.");
        });

    }
}
