using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.ViewModels;

namespace Testify.Application.Quizzes.Queries.GetQuizById;

public class GetQuizByIdQuery(Guid id) : IRequest<QuizWithQuestionsAndAnswersVm>
{
    public Guid Id { get; } = id;
    public class Handler : IRequestHandler<GetQuizByIdQuery, QuizWithQuestionsAndAnswersVm>
    {
        private readonly IQuizRepository quizRepository;

        public Handler(IQuizRepository quizRepository)
        {
            this.quizRepository = quizRepository;
        }

        public async Task<QuizWithQuestionsAndAnswersVm> Handle(GetQuizByIdQuery query, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetById(query.Id);

            return QuizWithQuestionsAndAnswersVm.From(quiz);
        }
    }
}
