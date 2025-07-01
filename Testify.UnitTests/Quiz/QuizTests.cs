using Testify.Domain.Constants;
using Testify.Domain.Entities;

namespace Testify.UnitTests.Quiz;

public class QuizTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var title = "Test Quiz";
        var description = "Description";
        var category = QuizCategoryType.Science;
        var isPrivate = true;
        var maxAttempts = 3;
        var timeLimit = TimeSpan.FromMinutes(30);

        // Act
        var quiz = new Domain.Entities.Quiz(title, description, category, isPrivate, maxAttempts, timeLimit);

        // Assert
        Assert.NotEqual(Guid.Empty, quiz.Id);
        Assert.Equal(title, quiz.Title);
        Assert.Equal(description, quiz.Description);
        Assert.Equal(category, quiz.Category);
        Assert.Equal(isPrivate, quiz.IsPrivate);
        Assert.Equal(maxAttempts, quiz.MaxAttempts);
        Assert.Equal(timeLimit, quiz.TimeLimit);
        Assert.Empty(quiz.Questions);
        Assert.Empty(quiz.Comments);
    }


    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenTitleIsNull()
    {
        // Arrange
        string title = null!;
        var description = "desc";
        var category = QuizCategoryType.GeneralKnowledge;
        var isPrivate = false;
        var maxAttempts = 1;
        TimeSpan? timeLimit = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new Domain.Entities.Quiz(title, description, category, isPrivate, maxAttempts, timeLimit));
    }

    [Fact]
    public void Create_ShouldReturnValidQuiz()
    {
        // Arrange
        var title = "My Quiz";
        var description = "desc";
        var category = QuizCategoryType.History;
        var isPrivate = false;
        var maxAttempts = 2;
        var timeLimit = TimeSpan.FromMinutes(15);

        // Act
        var quiz = Domain.Entities.Quiz.Create(title, description, category, isPrivate, maxAttempts, timeLimit);

        // Assert
        Assert.NotNull(quiz);
        Assert.Equal(title, quiz.Title);
        Assert.Equal(description, quiz.Description);
        Assert.Equal(category, quiz.Category);
        Assert.Equal(isPrivate, quiz.IsPrivate);
        Assert.Equal(maxAttempts, quiz.MaxAttempts);
        Assert.Equal(timeLimit, quiz.TimeLimit);
    }

    [Fact]
    public void Update_ShouldModifyProperties()
    {
        // Arrange
        var quiz = Domain.Entities.Quiz.Create("Old Title", "Old Desc", QuizCategoryType.Science, true, 1, TimeSpan.FromMinutes(10));
        var newTitle = "New Title";
        var newDescription = "New Desc";
        var newCategory = QuizCategoryType.History;
        var newIsPrivate = false;
        var newMaxAttempts = 5;
        var newTimeLimit = TimeSpan.FromMinutes(20);

        // Act
        quiz.Update(newTitle, newDescription, newCategory, newIsPrivate, newMaxAttempts, newTimeLimit);

        // Assert
        Assert.Equal(newTitle, quiz.Title);
        Assert.Equal(newDescription, quiz.Description);
        Assert.Equal(newCategory, quiz.Category);
        Assert.Equal(newIsPrivate, quiz.IsPrivate);
        Assert.Equal(newMaxAttempts, quiz.MaxAttempts);
        Assert.Equal(newTimeLimit, quiz.TimeLimit);
    }
}
