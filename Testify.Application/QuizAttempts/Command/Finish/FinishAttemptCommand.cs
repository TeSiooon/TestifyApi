using MediatR;
using Testify.Application.Common;
using Testify.Domain.Repositories;

namespace Testify.Application.QuizAttempts.Command.Finish;

public record FinishAttemptCommand(Guid AttemptId) : IRequest<Unit>
{
    public class Handler : IRequestHandler<FinishAttemptCommand, Unit>
    {
        private readonly IUserQuizAttemptRepository repo;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IUserQuizAttemptRepository repo, IUnitOfWork unitOfWork)
        {
            this.repo = repo;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(FinishAttemptCommand request, CancellationToken cancellationToken)
        {
            var attempt = await repo.GetByIdAsync(request.AttemptId, cancellationToken);

            attempt.Finish();
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

