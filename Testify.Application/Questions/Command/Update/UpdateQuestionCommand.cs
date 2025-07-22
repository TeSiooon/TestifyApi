using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;
using Testify.Application.Dtos;
using Testify.Domain.Entities;

namespace Testify.Application.Questions.Command.Update;

public record UpdateQuestionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public List<UpdateAnswerDto> Answers { get; set; } = [];

    public class Handler : IRequestHandler<UpdateQuestionCommand, Unit>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAnswerRepository answerRepository;

        public Handler(IQuestionRepository questionRepository, IUnitOfWork unitOfWork, IAnswerRepository answerRepository)
        {
            this.questionRepository = questionRepository;
            this.unitOfWork = unitOfWork;
            this.answerRepository = answerRepository;
        }
        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(request.Id, cancellationToken);
            question.Update(request.Text);
            var existingAnswers = question.Answers.ToDictionary(a => a.Id);

            foreach (var answerDto in request.Answers)
            {
                switch(answerDto.AnswerActionType)
                {
                    case AnswerActionType.Add:
                        var newAnswer = Answer.Create(request.Id, answerDto.Text, answerDto.IsCorrect);
                        existingAnswers[newAnswer.Id] = newAnswer;
                        await answerRepository.CreateAsync(newAnswer, cancellationToken);

                        question.AddAnswer(newAnswer);

                        break;

                    case AnswerActionType.Update:
                        if (!answerDto.Id.HasValue || !existingAnswers.TryGetValue(answerDto.Id.Value, out var answerToUpdate))
                            throw new InvalidOperationException($"Cannot update: Answer with ID {answerDto.Id} not found.");

                        answerToUpdate.Update(answerDto.Text, answerDto.IsCorrect);

                        break;

                    case AnswerActionType.Delete:
                        if (!answerDto.Id.HasValue || !existingAnswers.TryGetValue(answerDto.Id.Value, out var answerToDelete))
                            throw new InvalidOperationException($"Cannot delete: Answer with ID {answerDto.Id} not found.");

                        question.RemoveAnswer(answerToDelete);
                        existingAnswers.Remove(answerDto.Id.Value);

                        break;

                    default:
                        throw new InvalidOperationException($"Unknown ActionType: {answerDto.AnswerActionType}");
                }
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
