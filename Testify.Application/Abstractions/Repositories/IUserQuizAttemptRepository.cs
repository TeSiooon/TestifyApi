using Testify.Domain.Entities;

namespace Testify.Application.Abstractions.Repositories;

public interface IUserQuizAttemptRepository
{
    Task AddAsync(UserQuizAttempt attempt, CancellationToken cancellationToken = default);
    Task<UserQuizAttempt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountAttemptsAsync(Guid userId, Guid quizId, CancellationToken cancellationToken = default);
    Task<UserQuizAttempt?> GetWithQuizAndAnswersAsync(Guid attemptId, CancellationToken cancellationToken = default);
    Task<UserQuizAttempt?> GetWithAnswersAsync(Guid attemptId, CancellationToken cancellationToken = default);
    Task AddAnswerAsync(UserAnswer answer, CancellationToken cancellationToken = default);
    Task<List<UserQuizAttempt>> GetAllOpenAttemptsWithQuizAsync(CancellationToken cancellationToken = default);
}
