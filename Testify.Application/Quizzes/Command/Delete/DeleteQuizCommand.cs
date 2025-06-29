using MediatR;
using Testify.Application.Common;
using Testify.Domain.Repositories;

namespace Testify.Application.Quizzes.Command.Delete;

public class DeleteQuizCommand(Guid id) : IRequest<Unit>
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
