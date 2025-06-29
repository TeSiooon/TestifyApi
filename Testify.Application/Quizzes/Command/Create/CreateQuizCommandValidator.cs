using FluentValidation;

namespace Testify.Application.Quizzes.Command.Create;

public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
{
    public CreateQuizCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category is invalid");

        RuleFor(x => x.Questions)
            .NotEmpty()
            .WithMessage("Quiz must have at least one question.")
            .Must(qs => qs.Count <= 30)
            .WithMessage("Quiz cannot have more than 30 questions.");

        RuleForEach(x => x.Questions).ChildRules(question =>
        {
            question.RuleFor(q => q.Answers)
                .Must(ans => ans != null && ans.Count >= 2)
                .WithMessage("Each question should have at least two answers");

            question.RuleFor(q => q.Answers)
                .Must(ans => ans.Count <= 5)
                .WithMessage("Each question cannot have more than 5 answers.");

            question.RuleFor(q => q.Answers)
                .Must(ans => ans.Count(a => a.IsCorrect) == 1)
                .WithMessage("Each question must have exactly one correct answer.");
        });
    }
}
