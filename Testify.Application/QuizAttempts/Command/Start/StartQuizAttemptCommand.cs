using MediatR;
using Testify.Application.Common;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;

namespace Testify.Application.QuizAttempts.Command.Start;

public record StartQuizAttemptCommand(Guid quizId) : IRequest<Guid>
{
    public class Handler : IRequestHandler<StartQuizAttemptCommand, Guid>
    {
        private readonly IQuizRepository quizRepository;
        private readonly IUserQuizAttemptRepository userQuizAttemptRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public Handler(IQuizRepository quizRepository, IUserQuizAttemptRepository userQuizAttemptRepository
            , IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.quizRepository = quizRepository;
            this.userQuizAttemptRepository = userQuizAttemptRepository;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;

        }

        public async Task<Guid> Handle(StartQuizAttemptCommand request, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetById(request.quizId);

            var userId = currentUserService.UserId;

            var existingAttempts = await userQuizAttemptRepository.CountAttemptsAsync(userId, request.quizId, cancellationToken);

            if (quiz.MaxAttempts > 0 && existingAttempts >= quiz.MaxAttempts)
                throw new InvalidOperationException("Wykorzystano wszystkie próby.");

            var attempt = UserQuizAttempt.Create(userId, request.quizId);

            await userQuizAttemptRepository.AddAsync(attempt, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return attempt.Id;
        }
    }
}
