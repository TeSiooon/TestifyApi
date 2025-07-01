using FluentAssertions;
using Testify.Application.Dtos;
using Testify.Application.Quizzes.Command.Create;
using Testify.Application.Quizzes.Command.Delete;
using Testify.Domain.Constants;

namespace Testify.IntegrationTests.Quizzes.Commands.Delete;

[Collection("Testify Collection")]
public class DeleteQuizCommandTests
{
    private readonly TestifyFixture fixture;

    public DeleteQuizCommandTests(TestifyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Should_Delete_QuizAsync()
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

        var quizId = await fixture.ExecuteCommandAsync(command);


        var deleteQuizCommand = new DeleteQuizCommand(quizId);

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(deleteQuizCommand);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Should_Throw_When_Not_Existing_QuizAsync()
    {
        // Arrange
        var quizId = Guid.NewGuid();

        var command = new DeleteQuizCommand(quizId);
        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Asser
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
