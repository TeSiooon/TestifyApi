using FluentAssertions;
using FluentValidation;
using Testify.Application.Dtos;
using Testify.Application.Questions.Command.AddQuestionToQuiz;
using Testify.Application.Quizzes.Command.Create;
using Testify.Domain.Constants;

namespace Testify.IntegrationTests.Questions.Coomands.AddQuestionToQuiz;

[Collection("Testify Collection")]
public class AddQuestionToQuizCommandTests
{
    private readonly TestifyFixture fixture;

    public AddQuestionToQuizCommandTests(TestifyFixture fixture)
    {
        this.fixture = fixture;
    }

    private static QuestionDto ValidQuestionDto() => new()
    {
        Text = "Valid question?",
        Answers = new List<AnswerDto>
            {
                new() { Text = "Ans1", IsCorrect = true },
                new() { Text = "Ans2", IsCorrect = false }
            }
    };

    private async Task<Guid> CreateSampleQuizAsync()
    {
        var cmd = new CreateQuizCommand
        {
            Title = "Sample quiz",
            Description = "desc",
            Category = QuizCategoryType.GeneralKnowledge,
            IsPrivate = false,
            MaxAttempts = 3,
            TimeLimit = TimeSpan.FromMinutes(5),
            Questions = new List<QuestionDto>
            {
                new QuestionDto
                {
                    Text = "Q1?",
                    Answers = new List<AnswerDto>
                    {
                        new AnswerDto { Text = "A1", IsCorrect = true },
                        new AnswerDto { Text = "A2", IsCorrect = false }
                    }
                },
                new QuestionDto
                {
                    Text = "Q2?",
                    Answers = new List<AnswerDto>
                    {
                        new AnswerDto { Text = "B1", IsCorrect = true },
                        new AnswerDto { Text = "B2", IsCorrect = false }
                    }
                }
            }
        };

        return await fixture.ExecuteCommandAsync(cmd);
    }

    [Fact]
    public async Task Should_Add_Question_To_QuizAsync()
    {
        // Arrange
        var sampleQuizId = await CreateSampleQuizAsync();
        var addQuestionToQuizCommand = new AddQuestionToQuizCommand
        {
            QuizId = sampleQuizId,
            QuestionDto = new QuestionDto
            {
                Text = "Q3?",
                Answers = new List<AnswerDto>
                    {
                        new AnswerDto { Text = "C1", IsCorrect = true },
                        new AnswerDto { Text = "C2", IsCorrect = false }
                    }
            }
        };

        // Act
        var newQuestionId = await fixture.ExecuteCommandAsync(addQuestionToQuizCommand);

        // Assert
        var quiz = await fixture.QuizRepository.GetById(sampleQuizId);
        quiz.Should().NotBeNull();
        quiz!.Questions.Should().Contain(q =>
            q.Text == "Q3?" &&
            q.Answers.Count == 2 &&
            q.Answers.Count(a => a.IsCorrect) == 1
        );
    }

    [Fact]
    public async Task Should_Throw_When_Quiz_Not_ExistAsync()
    {
        // Arrange
        var sampleQuizId = Guid.NewGuid();
        var addQuestionToQuizCommand = new AddQuestionToQuizCommand
        {
            QuizId = sampleQuizId,
            QuestionDto = new QuestionDto
            {
                Text = "Q3?",
                Answers = new List<AnswerDto>
                    {
                        new AnswerDto { Text = "C1", IsCorrect = true },
                        new AnswerDto { Text = "C2", IsCorrect = false }
                    }
            }
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(addQuestionToQuizCommand);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Should_Have_Error_When_QuestionDto_Is_NullAsync()
    {
        // Arrange
        var command = new AddQuestionToQuizCommand
        {
            QuizId = Guid.NewGuid(),
            QuestionDto = null!
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_Have_Error_When_Question_Text_Is_EmptyAsync()
    {
        // Arrange
        var dto = ValidQuestionDto();
        dto.Text = "";

        var command = new AddQuestionToQuizCommand
        {
            QuizId = Guid.NewGuid(),
            QuestionDto = dto
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_Have_Error_When_Answers_List_Is_NullAsync()
    {
        // Arrange
        var dto = ValidQuestionDto();
        dto.Answers = null!;

        var command = new AddQuestionToQuizCommand
        {
            QuizId = Guid.NewGuid(),
            QuestionDto = dto
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }


    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Should_Have_Error_When_Too_Few_AnswersAsync(int count)
    {
        // Arrange
        var dto = new QuestionDto
        {
            Text = "Too few answers",
            Answers = Enumerable.Range(1, count)
                                .Select(i => new AnswerDto { Text = $"A{i}", IsCorrect = i == 1 })
                                .ToList()
        };

        var command = new AddQuestionToQuizCommand
        {
            QuizId = Guid.NewGuid(),
            QuestionDto = dto
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_Have_Error_When_Too_Many_AnswersAsync()
    {
        // Arrange
        var dto = new QuestionDto
        {
            Text = "Too many answers",
            Answers = Enumerable.Range(1, 6)
                .Select(i => new AnswerDto { Text = $"A{i}", IsCorrect = i == 1 })
                .ToList()
        };

        var command = new AddQuestionToQuizCommand
        {
            QuizId = Guid.NewGuid(),
            QuestionDto = dto
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_Have_Error_When_Not_Exactly_One_Correct_AnswerAsync()
    {
        // Arrange
        var dto = ValidQuestionDto();
        dto.Answers[1].IsCorrect = true; // Now two correct

        var command = new AddQuestionToQuizCommand
        {
            QuizId = Guid.NewGuid(),
            QuestionDto = dto
        };
        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_Have_Error_When_Answer_Text_Is_EmptyAsync()
    {
        // Arrange
        var dto = ValidQuestionDto();
        dto.Answers[0].Text = ""; // Empty answer text

        var command = new AddQuestionToQuizCommand
        {
            QuizId = Guid.NewGuid(),
            QuestionDto = dto
        };

        // Act
        Func<Task> act = async () => await fixture.ExecuteCommandAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}
