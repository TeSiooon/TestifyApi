using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;
using Testify.Domain.Constants;

namespace Testify.Application.Quizzes.Command.Update;

public record UpdateQuizCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public QuizCategoryType Category { get; set; }
    public bool IsPrivate { get; set; }
    public int MaxAttempts { get; set; }
    public TimeSpan? TimeLimit { get; set; }

    public class Handler : IRequestHandler<UpdateQuizCommand, Unit>
    {
        private readonly IQuizRepository quizRepository;
        private readonly IUnitOfWork unitOfWork;
        public Handler(IQuizRepository quizRepository, IUnitOfWork unitOfWork)
        {
            this.quizRepository = quizRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetById(request.Id);
            quiz.Update(request.Title, request.Description, request.Category,
                request.IsPrivate, request.MaxAttempts, request.TimeLimit);

            await unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
