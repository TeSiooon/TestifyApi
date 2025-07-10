using FluentAssertions;
using Testify.Application.QuizAttempts.Command.Finish;
using Testify.Application.QuizAttempts.Command.Start;
using Testify.Application.QuizAttempts.Command.Submit;
using Testify.Application.QuizAttempts.Queries.GetNextQuestion;
using Testify.IntegrationTests.Helpers;

namespace Testify.IntegrationTests.QuizAttempts;

[Collection("Testify Collection")]

public class QuizAttemptIntegrationTests
{
    private readonly TestifyFixture fixture;

    public QuizAttemptIntegrationTests(TestifyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Full_Quiz_Attempt_Flow_Work_CorrectlyAsync()
    {
        // Arrange
        var user = await fixture.GetTestUserAsync();
        fixture.SetUserContext(user.Id, user.UserName!, user.Email!);
        var (quizId, questionId) = await CreateQuizForTestHelper.CreateQuizWithQuestionAsync(fixture);

        // Act & Assert
        // 1) Start
        var attemptCommand = new StartQuizAttemptCommand(quizId);
        var attemptId = await fixture.ExecuteCommandAsync(attemptCommand);
        attemptId.Should().NotBeEmpty();

        // 2) Next -> should be next question
        var questionVm = await fixture.ExecuteQueryAsync(new GetNextQuestionQuery(attemptId));
        questionVm.Id.Should().Be(questionId);
        questionVm.Text.Should().Be("What is 2 + 2?");
        questionVm.Answers.Should().HaveCount(2);

        // 3) Submit correct answer
        var correctAnswerId = questionVm.Answers.Single(a => a.Text == "4").Id;
        await fixture.ExecuteCommandAsync(new SubmitAnswerCommand(attemptId, questionId, correctAnswerId));

        // 4) Next -> no more questions -> null
        var noMore = await fixture.ExecuteQueryAsync(new GetNextQuestionQuery(attemptId));
        noMore.Should().BeNull();

        // 5) Finish
        await fixture.ExecuteCommandAsync(new FinishAttemptCommand(attemptId));

        // 6) Attempt verification
        var attempt = await fixture.UserQuizAttemptRepository.GetByIdAsync(attemptId);
        attempt.Answers.Should().HaveCount(1);
        attempt.FinishedAt.Should().NotBeNull();

        // 7) User quiz result verifcation
        var userResult = await fixture.UserQuizResultRepository.GetUserQuizResultAsync(attempt.QuizId, attempt.UserId);
        userResult.Should().NotBeNull();
        userResult.Score.Should().Be(1);
        userResult.Attempts.Should().Be(1);
        userResult.CompletedDate.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
    }
}
