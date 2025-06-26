using System.Linq.Expressions;
using Testify.Application.Common;
using Testify.Domain.Entities;

namespace Testify.Application.ViewModels;

public sealed record QuestionVm(Guid Id, string Text, List<AnswerVm> Answers) : IViewModel<QuestionVm, Question>
{
    private static readonly Func<Question, QuestionVm> mapper = GetMapping().Compile();

    public static Expression<Func<Question, QuestionVm>> GetMapping()
    {
        return source => new QuestionVm(
            source.Id,
            source.Text,
            source.Answers.Select(a => new AnswerVm(a.Id, a.Text)).ToList()
            );
    }

    public static QuestionVm From(Question source)
    {
        return mapper(source);
    }

    public static QuestionVm? FromNullable(Question? source)
    {
        return source is null ? null : mapper(source);
    }
}