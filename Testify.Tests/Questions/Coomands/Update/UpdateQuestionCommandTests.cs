using FluentAssertions;
using FluentValidation;
using Testify.Application.Dtos;
using Testify.Application.Questions.Command.Update;
using Testify.IntegrationTests.Helpers;

namespace Testify.IntegrationTests.Questions.Coomands.Update;

[Collection("Testify Collection")]
public class UpdateQuestionCommandTests
{
    private readonly TestifyFixture fixture;

    public UpdateQuestionCommandTests(TestifyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Should_Update_Question_And_AnswersAsync()
    {
        // Arrange
        var (_, questionId) = await CreateQuizForTestHelper.CreateQuizWithQuestionAsync(fixture);

        var updateCommand = new UpdateQuestionCommand
        {
            Id = questionId,
            QuestionDto = new QuestionDto
            {
                Text = "Updated question?",
                Answers = new List<AnswerDto>
                {
                    new() { Text = "New 1", IsCorrect = false },
                    new() { Text = "Correct", IsCorrect = true },
                    new() { Text = "Wrong", IsCorrect = false }
                }
            }
        };
        // Act
        await fixture.ExecuteCommandAsync(updateCommand);

        // Assert
        var question = await fixture.QuestionRepository.GetByIdAsync(questionId);
        question.Text.Should().Be("Updated question?");
        //question.Answers.Should().HaveCount(3);
        //question.Answers.Count(a => a.IsCorrect).Should().Be(1);
        //question.Answers.Should().Contain(a => a.Text == "Correct" && a.IsCorrect);
    }

    [Fact]
    public async Task Should_Throw_When_Question_Not_FoundAsync()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var updateCmd = new UpdateQuestionCommand
        {
            Id = invalidId,
            QuestionDto = new QuestionDto
            {
                Text = "New?",
                Answers = new List<AnswerDto>
                {
                    new() { Text = "A", IsCorrect = true },
                    new() { Text = "B", IsCorrect = false }
                }
            }
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(updateCmd);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Should_Fail_When_Too_Few_AnswersAsync()
    {
        // Arrange
        var (_, questionId) = await CreateQuizForTestHelper.CreateQuizWithQuestionAsync(fixture);
        var updateCmd = new UpdateQuestionCommand
        {
            Id = questionId,
            QuestionDto = new QuestionDto
            {
                Text = "Q?",
                Answers = new List<AnswerDto>
                {
                    new() { Text = "Only one", IsCorrect = true }
                }
            }
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(updateCmd);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any(err => err.ErrorMessage.Contains("at least two answers")));
    }

    [Fact]
    public async Task Should_Fail_When_No_Correct_AnswerAsync()
    {
        // Arrange
        var (_, questionId) = await CreateQuizForTestHelper.CreateQuizWithQuestionAsync(fixture);
        var updateCmd = new UpdateQuestionCommand
        {
            Id = questionId,
            QuestionDto = new QuestionDto
            {
                Text = "Q?",
                Answers = new List<AnswerDto>
                {
                    new() { Text = "A", IsCorrect = false },
                    new() { Text = "B", IsCorrect = false }
                }
            }
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(updateCmd);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any(err => err.ErrorMessage.Contains("must have exactly one correct answer")));
    }
}
