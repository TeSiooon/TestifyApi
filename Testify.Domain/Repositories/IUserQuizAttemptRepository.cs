using Testify.Domain.Entities;

namespace Testify.Domain.Repositories;

public interface IUserQuizAttemptRepository
{
    Task AddAsync(UserQuizAttempt attempt, CancellationToken ct);
    Task<UserQuizAttempt?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<int> CountAttemptsAsync(Guid userId, Guid quizId, CancellationToken ct);
    Task<UserQuizAttempt?> GetWithQuizAndAnswersAsync(Guid attemptId, CancellationToken ct);
    Task<UserQuizAttempt?> GetWithAnswersAsync(Guid attemptId, CancellationToken ct);
    Task AddAnswerAsync(UserAnswer answer, CancellationToken ct);
}
