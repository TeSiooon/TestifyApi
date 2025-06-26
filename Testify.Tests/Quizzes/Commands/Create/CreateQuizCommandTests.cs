using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Testify.Application.Quizzes.Create;
using Testify.Domain.Constants;
using Testify.Domain.Repositories;

namespace Testify.IntegrationTests.Quizzes.Commands.Create;

[Collection("Testify Collection")]
public class CreateQuizCommandTests
{
    private readonly IMediator mediator;
    private readonly IQuizRepository quizRepo;

    public CreateQuizCommandTests(TestifyFixture fixture)
    {
        mediator = fixture.Services.GetRequiredService<IMediator>();
        quizRepo = fixture.Services.GetRequiredService<IQuizRepository>();
    }

    [Fact]
    public async Task Should_Create_QuizAsync()
    {
        // Arrange
        var command = new CreateQuizCommand
        {
            Title = "Test quiz",
            Description = "Test description",
            Category = QuizCategoryType.GeneralKnowledge,
            IsPrivate = false,
            MaxAttempts = 3,
            TimeLimit = TimeSpan.FromMinutes(10),
            Questions = new List<QuestionDto>
            {
                new()
                {
                    Text = "What is 2 + 2?",
                    Answers = new List<AnswerDto>
                    {
                        new() { Text = "4", IsCorrect = true },
                        new() { Text = "3", IsCorrect = false },
                        new() { Text = "5", IsCorrect = false }
                    }
                },
                new()
                {
                    Text = "Capital of France?",
                    Answers = new List<AnswerDto>
                    {
                        new() { Text = "Paris", IsCorrect = true },
                        new() { Text = "London", IsCorrect = false }
                    }
                }
            }
        };

        // Act
        var newQuizId = await mediator.Send(command);

        // Assert
        Assert.NotEqual(Guid.Empty, newQuizId);

        var createdQuiz = await quizRepo.GetById(newQuizId);

        Assert.Equal(command.Title, createdQuiz!.Title);
        Assert.Equal(command.Description, createdQuiz.Description);
        Assert.Equal(command.Category, createdQuiz.Category);
        Assert.Equal(command.IsPrivate, createdQuiz.IsPrivate);
        Assert.Equal(command.MaxAttempts, createdQuiz.MaxAttempts);
        Assert.Equal(command.TimeLimit, createdQuiz.TimeLimit);

        Assert.Equal(command.Questions.Count, createdQuiz.Questions.Count);

        // dto => data transer object
        // ent => entity

        foreach (var dtoQ in command.Questions)
        {
            var entQ = createdQuiz.Questions.Single(q => q.Text == dtoQ.Text);

            Assert.Equal(dtoQ.Answers.Count, entQ.Answers.Count);
            foreach(var dtoA in dtoQ.Answers)
            {
                var entA = entQ.Answers.Single(a => a.Text == dtoA.Text);
                Assert.Equal(dtoA.IsCorrect, entA.IsCorrect);
            }
        }
    }

    [Fact]
    public async Task Should_Throw_Validation_Exception_When_Create_With_Multiple_Correct_AnswersAsync()
    {
        // Arrange
        var command = new CreateQuizCommand
        {
            Title = "Test quiz",
            Description = "Test description",
            Category = QuizCategoryType.GeneralKnowledge,
            IsPrivate = false,
            MaxAttempts = 3,
            TimeLimit = TimeSpan.FromMinutes(10),
            Questions = new List<QuestionDto>
            {
                new()
                {
                    Text = "What is 2 + 2?",
                    Answers = new List<AnswerDto>
                    {
                        new() { Text = "4", IsCorrect = true },
                        new() { Text = "3", IsCorrect = true },
                        new() { Text = "5", IsCorrect = false }
                    }
                },
                new()
                {
                    Text = "Capital of France?",
                    Answers = new List<AnswerDto>
                    {
                        new() { Text = "Paris", IsCorrect = true },
                        new() { Text = "London", IsCorrect = false }
                    }
                }
            }
        };
        // Act
        Func<Task> act = async () => await mediator.Send(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_Throw_When_Too_Many_QuestionsAsync()
    {
        // Arrange
        var command = new CreateQuizCommand
        {
            Title = "Too many questions",
            Description = "Test",
            Category = QuizCategoryType.GeneralKnowledge,
            IsPrivate = false,
            MaxAttempts = 3,
            Questions = Enumerable.Range(1, 31).Select(i => new QuestionDto
            {
                Text = $"Question {i}",
                Answers = new List<AnswerDto>
            {
                new() { Text = "Answer A", IsCorrect = true },
                new() { Text = "Answer B", IsCorrect = false }
            }
            }).ToList()
        };

        // Act
        Func<Task> act = async () => await mediator.Send(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

}
