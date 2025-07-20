using Testify.Domain.Entities;

namespace Testify.Application.Abstractions.Repositories;

public interface IUserQuizResultRepository
{
    Task AddAsync(UserQuizResult result, CancellationToken cancellationToken = default);
    Task<UserQuizResult> GetUserQuizResultAsync(Guid quizId, Guid userId, CancellationToken cancellationToken = default);
    Task<UserQuizResult> GetUserQuizResultAsync(Guid id, CancellationToken cancellationToken = default);
}
