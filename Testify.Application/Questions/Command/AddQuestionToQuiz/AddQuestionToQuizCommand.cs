using MediatR;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;
using Testify.Application.Dtos;
using Testify.Domain.Entities;

namespace Testify.Application.Questions.Command.AddQuestionToQuiz;

public record AddQuestionToQuizCommand : IRequest<Guid>
{
    public Guid QuizId { get; set; }

    public QuestionDto QuestionDto { get; set; }

    public class Handler : IRequestHandler<AddQuestionToQuizCommand, Guid>
    {
        private readonly IQuizRepository quizRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IQuizRepository quizRepository, IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
        {
            this.quizRepository = quizRepository;
            this.questionRepository = questionRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddQuestionToQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetById(request.QuizId);

            var question = Question.Create(quiz.Id, request.QuestionDto.Text);

            foreach (var answerDto in request.QuestionDto.Answers)
            {
                var answer = Answer.Create(question.Id, answerDto.Text, answerDto.IsCorrect);
                question.AddAnswer(answer);
            }

            await questionRepository.CreateAsync(question, cancellationToken);

            quiz.AddQuestion(question);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return question.Id;
        }
    }
}
