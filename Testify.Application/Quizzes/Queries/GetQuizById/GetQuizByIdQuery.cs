using MediatR;
using Testify.Domain.Repositories;

namespace Testify.Application.Quizzes.Queries.GetQuizById;

public class GetQuizByIdQuery(Guid id) : IRequest<QuizVm>
{
    public Guid Id { get; } = id;
    public class Handler : IRequestHandler<GetQuizByIdQuery, QuizVm>
    {
        private readonly IQuizRepository quizRepository;

        public Handler(IQuizRepository quizRepository)
        {
            this.quizRepository = quizRepository;
        }

        public async Task<QuizVm> Handle(GetQuizByIdQuery query, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetById(query.Id);

            return QuizVm.From(quiz);
        }
    }
}
