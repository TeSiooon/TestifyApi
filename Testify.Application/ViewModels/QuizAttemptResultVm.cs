using System.Linq.Expressions;
using Testify.Application.Common;
using Testify.Domain.Entities;

namespace Testify.Application.ViewModels;

public sealed record QuizAttemptResultVm(Guid Id, int Score, int? MaxPoints, int Attempts) : IViewModel<QuizAttemptResultVm, UserQuizResult>
{
    private static readonly Func<UserQuizResult, QuizAttemptResultVm> mapper = GetMapping().Compile();

    public static Expression<Func<UserQuizResult, QuizAttemptResultVm>> GetMapping()
    {
        return source => new QuizAttemptResultVm(
            source.Id,
            source.Score,
            source.Quiz.Questions.Count(),
            source.Attempts
            );
    }

    public static QuizAttemptResultVm From(UserQuizResult source)
    {
        return mapper(source);
    }

    public static QuizAttemptResultVm? FromNullable(UserQuizResult? source)
    {
        return source is null ? null : mapper(source);
    }
}
