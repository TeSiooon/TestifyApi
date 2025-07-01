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

        var question = await fixture.QuestionRepository.GetByIdAsync(questionId);
        var existingAnswers = question.Answers.ToList();
        var updatedAnswers = new List<UpdateAnswerDto>
            {
                new()
                {
                    AnswerActionType = Application.Common.AnswerActionType.Update,
                    Id = existingAnswers[0].Id,
                    Text = "Updated 1",
                    IsCorrect = false
                },
                new()
                {
                    AnswerActionType = Application.Common.AnswerActionType.Update,
                    Id = existingAnswers[1].Id,
                    Text = "Now the correct one",
                    IsCorrect = true
                },
                new()
                {
                    AnswerActionType = Application.Common.AnswerActionType.Add,
                    Id = null,
                    Text = "Brand new answer",
                    IsCorrect = false
                }
            };
        var updateCommand = new UpdateQuestionCommand
        {
            Id = questionId,
            Text = "Updated question?",
            Answers = updatedAnswers
        };
        // Act
        await fixture.ExecuteCommandAsync(updateCommand);

        // Assert
        var updatedQuestion = await fixture.QuestionRepository.GetByIdAsync(questionId);
        updatedQuestion.Text.Should().Be("Updated question?");
        updatedQuestion.Answers.Should().HaveCount(3);
        updatedQuestion.Answers.Should().ContainSingle(a => a.Text == "Now the correct one" && a.IsCorrect);
        updatedQuestion.Answers.Should().ContainSingle(a => a.Text == "Brand new answer");
    }

    [Fact]
    public async Task Should_Throw_When_Question_Not_FoundAsync()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var updateCmd = new UpdateQuestionCommand
        {
            Id = invalidId,
            Text = "New?",
            Answers = new List<UpdateAnswerDto>
            {
                new UpdateAnswerDto { 
                    AnswerActionType = Application.Common.AnswerActionType.Update,
                    Id = Guid.NewGuid(), Text = "New 1", IsCorrect = false },
                new UpdateAnswerDto {
                     AnswerActionType = Application.Common.AnswerActionType.Update,
                    Id = Guid.NewGuid(), Text = "Correct", IsCorrect = true },
                new UpdateAnswerDto {AnswerActionType = Application.Common.AnswerActionType.Update, 
                    Id = Guid.NewGuid(), Text = "Wrong", IsCorrect = false }
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
            Text = "Q?",
            Answers = new List<UpdateAnswerDto>
            {
                new UpdateAnswerDto { Id = Guid.NewGuid(), Text = "Wrong", IsCorrect = false }
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
            Text = "Q?",
            Answers = new List<UpdateAnswerDto>
            {
                new UpdateAnswerDto { Id = Guid.NewGuid(), Text = "New 1", IsCorrect = false },
                new UpdateAnswerDto { Id = Guid.NewGuid(), Text = "Correct", IsCorrect = false },
                new UpdateAnswerDto { Id = Guid.NewGuid(), Text = "Wrong", IsCorrect = false }
            }
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(updateCmd);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any(err => err.ErrorMessage.Contains("must have exactly one correct answer")));
    }
}
