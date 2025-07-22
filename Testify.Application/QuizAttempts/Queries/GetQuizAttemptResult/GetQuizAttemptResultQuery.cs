using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.ViewModels;

namespace Testify.Application.QuizAttempts.Queries.GetQuizAttemptResult;

public record GetQuizAttemptResultQuery(Guid AttemptId) :IRequest<QuizAttemptResultVm>
{
    public class Handler : IRequestHandler<GetQuizAttemptResultQuery, QuizAttemptResultVm>
    {
        private readonly IUserQuizResultRepository resultRepository;
        public Handler(IUserQuizResultRepository resultRepository)
        {
            this.resultRepository = resultRepository;
        }
        public async Task<QuizAttemptResultVm> Handle(GetQuizAttemptResultQuery request, CancellationToken cancellationToken)
        {
            var result = await resultRepository.GetUserQuizResultAsync(request.AttemptId, cancellationToken);
            return QuizAttemptResultVm.From(result);
        }
    }
}
