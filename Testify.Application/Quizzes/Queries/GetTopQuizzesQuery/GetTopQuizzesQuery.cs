using MediatR;
using Testify.Application.ViewModels;
using Testify.Domain.Repositories;

namespace Testify.Application.Quizzes.Queries.GetTopQuizzesQuery;

public class GetTopQuizzesQuery : IRequest<List<QuizVm>>
{
    public class Handler : IRequestHandler<GetTopQuizzesQuery, List<QuizVm>>
    {
        private readonly IQuizRepository quizRepository;
        public Handler(IQuizRepository quizRepository)
        {
            this.quizRepository = quizRepository;
        }
        public async Task<List<QuizVm>> Handle(GetTopQuizzesQuery request, CancellationToken cancellationToken)
        {
            var quizzes = await quizRepository.GetTopQuizzesAsync(5, cancellationToken);
            return quizzes.Select(QuizVm.From).ToList();
        }
    }
}
