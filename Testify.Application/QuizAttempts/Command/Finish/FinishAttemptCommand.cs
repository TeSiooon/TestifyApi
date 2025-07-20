using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;
using Testify.Domain.Entities;

namespace Testify.Application.QuizAttempts.Command.Finish;

public record FinishAttemptCommand(Guid AttemptId) : IRequest<Guid>
{
    public class Handler : IRequestHandler<FinishAttemptCommand, Guid>
    {
        private readonly IUserQuizAttemptRepository repo;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserQuizResultRepository userQuizResultRepository;

        public Handler(IUserQuizAttemptRepository repo, IUnitOfWork unitOfWork, IUserQuizResultRepository userQuizResultRepository)
        {
            this.repo = repo;
            this.unitOfWork = unitOfWork;
            this.userQuizResultRepository = userQuizResultRepository;
        }

        public async Task<Guid> Handle(FinishAttemptCommand request, CancellationToken cancellationToken)
        {
            var attempt = await repo.GetByIdAsync(request.AttemptId, cancellationToken);

            attempt.Finish();

            var correctCount = attempt.Answers
                .Count(ua => ua.SelectedAnswer.IsCorrect);

            var attemptsCount = await repo
                .CountAttemptsAsync(attempt.UserId, attempt.QuizId, cancellationToken);

            var result = UserQuizResult.Create(
                userId: attempt.UserId,
                quizId: attempt.QuizId,
                score: correctCount,
                attempts: attemptsCount,
                completedDate: attempt.FinishedAt!.Value
            );

            await userQuizResultRepository.AddAsync(result, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Id;
        }
    }
}

