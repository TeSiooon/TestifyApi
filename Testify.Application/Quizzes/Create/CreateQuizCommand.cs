using MediatR;
using Testify.Domain.Constants;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;

namespace Testify.Application.Quizzes.Create;

public class CreateQuizCommand : IRequest<Guid>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public QuizCategoryType Category { get; set; }
    public bool IsPrivate { get; set; }
    public int MaxAttempts { get; set; }
    public TimeSpan? TimeLimit { get; set; }

    public List<QuestionDto> Questions { get; set; } = [];

    public class Handler : IRequestHandler<CreateQuizCommand, Guid>
    {
        private readonly IQuizRepository quizRepository;
        public Handler(IQuizRepository quizRepository)
        {
            this.quizRepository = quizRepository;
        }

        public async Task<Guid> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = Quiz.Create(request.Title, request.Description, request.Category,
                       request.IsPrivate, request.MaxAttempts, request.TimeLimit);

            foreach (var questionDto in request.Questions)
            {
                var question = Question.Create(quiz.Id, questionDto.Text);

                foreach (var answerDto in questionDto.Answers)
                {
                    var answer = Answer.Create(quiz.Id, answerDto.Text, answerDto.IsCorrect);
                    question.AddAnswer(answer);
                }

                quiz.AddQuestion(question);
            }

            await quizRepository.Create(quiz);

            return quiz.Id;
        }
    }
}
