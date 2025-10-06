using FluentAssertions;
using Testify.Application.QuizReports.Command.Create;
using Testify.IntegrationTests.Helpers;

namespace Testify.IntegrationTests.QuizReports;

[Collection("Testify Collection")]

public class QuizReportIntegrationTests
{
    private readonly TestifyFixture fixture;
    public QuizReportIntegrationTests(TestifyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Should_Add_Report_To_QuizAsync()
    {
        // Arrange
        var user = await fixture.GetTestUserAsync();
        fixture.SetUserContext(user.Id, user.UserName!, user.Email!);
        var (quizId, _) = await CreateQuizForTestHelper.CreateQuizWithQuestionAsync(fixture);

        // Act
        var reportCommand = new CreateQuizReportCommand
        {
            QuizId = quizId,
            Reason = "The quiz contains inappropriate content."
        };

        var reportId = await fixture.ExecuteCommandAsync(reportCommand);

        // Assert
        var report = await fixture.QuizReportRepository.GetByIdAsync(reportId);

        report.Should().NotBeNull();  
        report.Id.Should().Be(reportId);
        report.QuizId.Should().Be(quizId);
        report.ReportedById.Should().Be(user.Id);
        report.Reason.Should().Be("The quiz contains inappropriate content.");

        var quiz = await fixture.QuizRepository.GetById(quizId);
        quiz.Should().NotBeNull();
        quiz.Reports.Should().Contain(report);
    }
}
