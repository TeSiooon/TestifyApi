using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Testify.Application.Common;
using Testify.Application.ViewModels;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;

namespace Testify.Application.QuizAttempts.Command.Submit;

public record SubmitAnswerCommand(Guid AttemptId, Guid QuestionId, Guid SelectedAnswerId) : IRequest<Unit>
{
    public class Handler : IRequestHandler<SubmitAnswerCommand,Unit>
    {
        private readonly IUserQuizAttemptRepository attemptRepo;
        private readonly IUnitOfWork unitOfWork;
        public Handler(IUserQuizAttemptRepository attemptRepo, IUnitOfWork unitOfWork)
        {
            this.attemptRepo = attemptRepo;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
        {
            var attempt = await attemptRepo.GetWithAnswersAsync(request.AttemptId, cancellationToken);

            var alreadyAnswered = attempt.Answers.Any(a => a.QuestionId == request.QuestionId);
            if (alreadyAnswered)
                throw new InvalidOperationException("Pytanie już zostało rozwiązane.");

            var userAnswer = UserAnswer.Create(request.AttemptId, request.QuestionId, request.SelectedAnswerId);

            attempt.AddAnswer(userAnswer);

            await attemptRepo.AddAnswerAsync(userAnswer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}