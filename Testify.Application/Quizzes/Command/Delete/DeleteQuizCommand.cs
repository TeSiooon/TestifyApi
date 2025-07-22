using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;

namespace Testify.Application.Quizzes.Command.Delete;

public record DeleteQuizCommand(Guid id) : IRequest<Unit>
{
    public Guid Id { get; } = id;

    public class Handler : IRequestHandler<DeleteQuizCommand, Unit>
    {
        private readonly IQuizRepository quizRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IQuizRepository quizRepository, IUnitOfWork unitOfWork)
        {
            this.quizRepository = quizRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
        {
            await quizRepository.DeleteAsync(request.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
