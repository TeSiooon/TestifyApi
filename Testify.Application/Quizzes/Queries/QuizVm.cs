using System.Linq.Expressions;
using Testify.Application.Common;
using Testify.Domain.Entities;

namespace Testify.Application.Quizzes.Queries;

public sealed record QuizVm(
    Guid Id,
    string Title
) : IViewModel<QuizVm, Quiz>
{
    private static readonly Func<Quiz, QuizVm> mapper = GetMapping().Compile();

    public static Expression<Func<Quiz, QuizVm>> GetMapping()
    {
        return source => new QuizVm(
            source.Id,
            source.Title
            );
    }

    public static QuizVm From(Quiz source)
    {
        return mapper( source );
    }

    public static QuizVm? FromNullable(Quiz? source)
    {
        return source is null ? null : mapper( source );
    }
}
