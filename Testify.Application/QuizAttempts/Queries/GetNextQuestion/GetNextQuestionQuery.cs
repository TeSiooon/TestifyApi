using MediatR;
using Testify.Application.ViewModels;
using Testify.Domain.Repositories;

namespace Testify.Application.QuizAttempts.Queries.GetNextQuestion;

public record GetNextQuestionQuery(Guid AttemptId) : IRequest<QuestionVm>
{
    public class Handler : IRequestHandler<GetNextQuestionQuery, QuestionVm>
    {
        private readonly IUserQuizAttemptRepository userQuizAttemptRepository;
        public Handler(IUserQuizAttemptRepository userQuizAttemptRepository)
        {
            this.userQuizAttemptRepository = userQuizAttemptRepository;
        }

        public async Task<QuestionVm> Handle(GetNextQuestionQuery request, CancellationToken cancellationToken)
        {
            var attempt = await userQuizAttemptRepository.GetWithQuizAndAnswersAsync(request.AttemptId, cancellationToken);

            var answeredIds = attempt.Answers.Select(ua => ua.QuestionId).ToHashSet();
            var next = attempt.Quiz.Questions.FirstOrDefault(q => !answeredIds.Contains(q.Id));

            return QuestionVm.From(next);
        }
    }
}
