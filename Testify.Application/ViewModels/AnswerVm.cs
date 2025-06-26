using System.Linq.Expressions;
using Testify.Application.Common;
using Testify.Domain.Entities;

namespace Testify.Application.ViewModels;

/// <summary>
/// Public-facing Answer view model. Does NOT include information whether answer is correct.
/// </summary>
public sealed record AnswerVm(Guid Id, string Text ) : IViewModel<AnswerVm, Answer>
{
    private static readonly Func<Answer, AnswerVm> mapper = GetMapping().Compile();

    public static Expression<Func<Answer, AnswerVm>> GetMapping()
    {
        return source => new AnswerVm(
            source.Id,
            source.Text
            );
    }

    public static AnswerVm From(Answer source)
    {
        return mapper(source);
    }

    public static AnswerVm? FromNullable(Answer? source)
    {
        return source is null ? null : mapper(source);
    }
}

