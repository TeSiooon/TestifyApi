using Testify.Application.Dtos;
using Testify.Application.Quizzes.Command.Create;
using Testify.Domain.Constants;

namespace Testify.IntegrationTests.Helpers;

public static class CreateQuizForTestHelper
{
    public static async Task<(Guid quizId, Guid questionId)> CreateQuizWithQuestionAsync(TestifyFixture fixture)
    {
        var command = new CreateQuizCommand
        {
            Title = "Test Quiz",
            Description = "Test Desc",
            Category = QuizCategoryType.GeneralKnowledge,
            IsPrivate = false,
            MaxAttempts = 3,
            TimeLimit = TimeSpan.FromMinutes(10),
            Questions = new List<QuestionDto>
            {
                new QuestionDto
                {
                    Text = "What is 2 + 2?",
                    Answers = new List<AnswerDto>
                    {
                        new() { Text = "4", IsCorrect = true },
                        new() { Text = "3", IsCorrect = false }
                    }
                }
            }
        };

        var quizId = await fixture.ExecuteCommandAsync(command);
        var quiz = await fixture.QuizRepository.GetById(quizId);
        var questionId = quiz!.Questions.First().Id;

        return (quizId, questionId);
    }
}
