using MediatR;
using Testify.Application.Common;
using Testify.Application.Dtos;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;

namespace Testify.Application.Questions.Command.Update;

public record UpdateQuestionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public QuestionDto QuestionDto { get; set; } // question with answers

    public class Handler : IRequestHandler<UpdateQuestionCommand, Unit>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
        {
            this.questionRepository = questionRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(request.Id);
            question.Update(request.QuestionDto.Text);

            question.ClearAnswers();

            foreach (var answerDto in request.QuestionDto.Answers)
            {
                var answer = Answer.Create(question.Id, answerDto.Text, answerDto.IsCorrect);
                question.AddAnswer(answer);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
