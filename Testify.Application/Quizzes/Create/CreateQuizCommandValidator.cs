using FluentValidation;

namespace Testify.Application.Quizzes.Create;

public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
{
    // todo dodac walidatory dla tworzenia quizu
    public CreateQuizCommandValidator()
    {
        RuleFor(x => x.Questions)
            .Must(qs => qs.Count <= 30)
            .WithMessage("Quiz cannot have more than 30 questions.");

        RuleForEach(x => x.Questions).ChildRules(question =>
        {
            question.RuleFor(q => q.Answers)
                .Must(ans => ans.Count <= 5)
                .WithMessage("Each question cannot have more than 5 answers.");

            question.RuleFor(q => q.Answers)
                .Must(ans => ans.Count(a => a.IsCorrect) == 1)
                .WithMessage("Each question must have exactly one correct answer.");
        });
    }
}
