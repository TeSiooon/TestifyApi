using FluentAssertions;
using Testify.Application.Quizzes.Command.Create;
using Testify.Application.Quizzes.Command.Update;
using Testify.Domain.Constants;

namespace Testify.IntegrationTests.Quizzes.Commands.Update;

[Collection("Testify Collection")]
public class UpdateQuizCommandTests
{
    private readonly TestifyFixture fixture;

    public UpdateQuizCommandTests(TestifyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Should_Update_QuizAsync()
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
        
        var updateQuizCommand = new UpdateQuizCommand
        {
            Id = quizId,
            Title = "New Title",
            Description = "New Desc",
            Category = QuizCategoryType.Science,
            IsPrivate = true,
            MaxAttempts = 5,
            TimeLimit = TimeSpan.FromMinutes(30)
        };

        // Act
        await fixture.ExecuteCommandAsync(updateQuizCommand);

        // Assert
        var updatedQuiz = await fixture.QuizRepository.GetById(quizId);

        updatedQuiz.Should().NotBeNull();
        updatedQuiz!.Title.Should().Be("New Title");
        updatedQuiz.Description.Should().Be("New Desc");
        updatedQuiz.Category.Should().Be(Testify.Domain.Constants.QuizCategoryType.Science);
        updatedQuiz.IsPrivate.Should().BeTrue();
        updatedQuiz.MaxAttempts.Should().Be(5);
        updatedQuiz.TimeLimit.Should().Be(TimeSpan.FromMinutes(30));
    }

    [Fact]
    public async Task Should_Throw_Exception_For_NonExistisingQuizAsync()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var updateQuizCommand = new UpdateQuizCommand
        {
            Id = quizId,
            Title = "New Title",
            Description = "New Desc",
            Category = QuizCategoryType.Science,
            IsPrivate = true,
            MaxAttempts = 5,
            TimeLimit = TimeSpan.FromMinutes(30)
        };
        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(updateQuizCommand);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
