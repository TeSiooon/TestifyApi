using MediatR;
using Testify.Domain.Constants;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;

namespace Testify.Application.Quizzes.Create;

public class CreateQuizCommand : IRequest<Guid>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public QuizCategoryType Category { get; set; }
    public bool IsPrivate { get; set; }
    public int MaxAttempts { get; set; }
    public TimeSpan? TimeLimit { get; set; }

    public class Handler : IRequestHandler<CreateQuizCommand, Guid>
    {
        private readonly IQuizRepository quizRepository;
        public Handler(IQuizRepository quizRepository)
        {
            this.quizRepository = quizRepository;
        }

        public async Task<Guid> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = Quiz.Create(request.Title, request.Description, request.Category,
                       request.IsPrivate, request.MaxAttempts, request.TimeLimit);

            await quizRepository.Create(quiz);

            return quiz.Id;
        }
    }
}
