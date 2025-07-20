using MediatR;
using Microsoft.EntityFrameworkCore;
using Testify.Application.Dtos;
using Testify.Application.Quizzes.Command.Create;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Seeders;

public class QuizSeeder
{
    private readonly TestifyDbContext dbContext;
    private readonly IMediator mediator;
    public QuizSeeder(TestifyDbContext dbContext, IMediator mediator)
    {
        this.dbContext = dbContext;
        this.mediator = mediator;
    }
    public async Task SeedQuizzesAsync()
    {
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            await dbContext.Database.MigrateAsync();
        if (!await dbContext.Database.CanConnectAsync())
            return;

        var existingQuizzes = await dbContext.Quizzes.AnyAsync();
        if (existingQuizzes)
            return;

        for (int i = 1; i <= 15; i++)
        {
            var command = new CreateQuizCommand
            {
                Title = $"Sample Quiz {i}",
                Description = "This is a sample quiz for testing purposes.",
                Category = Domain.Constants.QuizCategoryType.GeneralKnowledge,
                IsPrivate = false,
                MaxAttempts = 5,
                TimeLimit = TimeSpan.FromMinutes(30),
                Questions = GenerateQuestions(1)
            };
            var quizId = await mediator.Send(command);
        }
    }

    private List<QuestionDto> GenerateQuestions(int seed)
    {
        var questions = new List<QuestionDto>();

        for (int i = 1; i <= 3; i++)
        {
            int answerCount = (seed + i) % 3 + 2;

            var answers = new List<AnswerDto>();
            for (int j = 1; j <= answerCount; j++)
            {
                answers.Add(new AnswerDto
                {
                    Text = $"Answer {j} for Q{i} in Quiz#{seed}",
                    IsCorrect = j == 1
                });
            }

            questions.Add(new QuestionDto
            {
                Text = $"Question {i} for Quiz#{seed}",
                Answers = answers
            });
        }

        return questions;
    }
}
