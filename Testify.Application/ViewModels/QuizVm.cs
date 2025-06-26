using System.Linq.Expressions;
using Testify.Application.Common;
using Testify.Domain.Constants;
using Testify.Domain.Entities;

namespace Testify.Application.ViewModels;

public sealed record QuizVm(
    Guid Id,
    string Title,
    string Description,
    QuizCategoryType Category,
    int MaxAttempts,
    TimeSpan? TimeLimit
) : IViewModel<QuizVm, Quiz>
{
    private static readonly Func<Quiz, QuizVm> mapper = GetMapping().Compile();

    public static Expression<Func<Quiz, QuizVm>> GetMapping()
    {
        return source => new QuizVm(
            source.Id,
            source.Title,
            source.Description,
            source.Category,
            source.MaxAttempts,
            source.TimeLimit
            );
    }

    public static QuizVm From(Quiz source)
    {
        return mapper(source);
    }

    public static QuizVm? FromNullable(Quiz? source)
    {
        return source is null ? null : mapper(source);
    }
}
