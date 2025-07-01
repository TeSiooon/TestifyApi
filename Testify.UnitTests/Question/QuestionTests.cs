using Testify.Domain.Entities;

namespace Testify.UnitTests.Question;

public class QuestionTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var text = "Sample question?";

        // Act
        var question = new Domain.Entities.Question(quizId, text);

        // Assert
        Assert.NotEqual(Guid.Empty, question.Id);
        Assert.Equal(quizId, question.QuizId);
        Assert.Equal(text, question.Text);
        Assert.Empty(question.Answers);
    }

    [Fact]
    public void Create_ShouldReturnQuestionWithCorrectValues()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var text = "Question text";

        // Act
        var question = Domain.Entities.Question.Create(quizId, text);

        // Assert
        Assert.NotNull(question);
        Assert.Equal(quizId, question.QuizId);
        Assert.Equal(text, question.Text);
        Assert.NotEqual(Guid.Empty, question.Id);
    }

    [Fact]
    public void Update_ShouldChangeText()
    {
        // Arrange
        var question = Domain.Entities.Question.Create(Guid.NewGuid(), "Old text");
        var newText = "Updated question text";

        // Act
        question.Update(newText);

        // Assert
        Assert.Equal(newText, question.Text);
    }

    [Fact]
    public void Update_ShouldThrowArgumentNullException_WhenTextIsNull()
    {
        // Arrange
        var question = Domain.Entities.Question.Create(Guid.NewGuid(), "Old text");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => question.Update(null!));
    }

    [Fact]
    public void AddAnswer_ShouldAddAnswerToAnswersCollection()
    {
        // Arrange
        var question = Domain.Entities.Question.Create(Guid.NewGuid(), "Question?");
        var answer = new Answer(Guid.NewGuid(), "Answer text", false);

        // Act
        question.AddAnswer(answer);

        // Assert
        Assert.Single(question.Answers);
        Assert.Contains(answer, question.Answers);
    }

    [Fact]
    public void RemoveAnswer_ShouldRemoveAnswerFromAnswersCollection()
    {
        // Arrange
        var question = Domain.Entities.Question.Create(Guid.NewGuid(), "Question?");
        var answer = new Answer(Guid.NewGuid(), "Answer text", false);
        question.AddAnswer(answer);

        // Act
        question.RemoveAnswer(answer);

        // Assert
        Assert.Empty(question.Answers);
    }
}
