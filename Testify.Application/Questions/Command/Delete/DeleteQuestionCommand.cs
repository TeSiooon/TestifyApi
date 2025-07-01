using MediatR;
using Testify.Application.Common;
using Testify.Domain.Repositories;

namespace Testify.Application.Questions.Command.Delete;

public record DeleteQuestionCommand(Guid id) : IRequest<Unit>
{
    public Guid QuestionId { get; } = id;

    public class Handler : IRequestHandler<DeleteQuestionCommand, Unit>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
        {
            this.questionRepository = questionRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            await questionRepository.DeleteAsync(request.QuestionId, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
