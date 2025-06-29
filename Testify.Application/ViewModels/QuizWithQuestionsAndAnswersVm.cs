using System.Linq.Expressions;
using Testify.Application.Common;
using Testify.Domain.Constants;
using Testify.Domain.Entities;

namespace Testify.Application.ViewModels;

public sealed record QuizWithQuestionsAndAnswersVm(
    Guid Id,
    string Title,
    string Description,
    QuizCategoryType Category,
    int MaxAttempts,
    TimeSpan? TimeLimit,
    List<QuestionVm> Questions) : IViewModel<QuizWithQuestionsAndAnswersVm, Quiz>
{
    private static readonly Func<Quiz, QuizWithQuestionsAndAnswersVm> mapper = GetMapping().Compile();

    public static Expression<Func<Quiz, QuizWithQuestionsAndAnswersVm>> GetMapping()
    {
        return source => new QuizWithQuestionsAndAnswersVm(
            source.Id,
            source.Title,
            source.Description,
            source.Category,
            source.MaxAttempts,
            source.TimeLimit,
            source.Questions.Select(question => new QuestionVm(
                question.Id,
                question.Text,
                question.Answers.Select(answer => new AnswerVm(
                    answer.Id,
                    answer.Text
                )).ToList()
            )).ToList()
            );
    }

    public static QuizWithQuestionsAndAnswersVm From(Quiz source)
    {
        return mapper(source);
    }

    public static QuizWithQuestionsAndAnswersVm? FromNullable(Quiz? source)
    {
        return source is null ? null : mapper(source);
    }
}
