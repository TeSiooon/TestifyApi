using FluentAssertions;
using Testify.Application.Questions.Command.Delete;
using Testify.IntegrationTests.Helpers;

namespace Testify.IntegrationTests.Questions.Coomands.Delete;

[Collection("Testify Collection")]

public class DeleteQuestionCommandTests
{
    private readonly TestifyFixture fixture;

    public DeleteQuestionCommandTests(TestifyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Should_Delete_Question_From_QuizAsync()
    {
        // Arrange
        (Guid quizId, Guid questionId) = await CreateQuizForTestHelper.CreateQuizWithQuestionAsync(fixture);
        var deleteCommand = new DeleteQuestionCommand(questionId);

        // Act
        await fixture.ExecuteCommandAsync(deleteCommand);

        // Assert
        var quiz = await fixture.QuizRepository.GetById(quizId);
        quiz.Should().NotBeNull();
        quiz!.Questions.Should().NotContain(q => q.Id == questionId);
    }


    [Fact]
    public async Task Should_Throw_When_Deleting_NonExisting_Question()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        var command = new DeleteQuestionCommand(nonExistingId);

        // Act
        var act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
