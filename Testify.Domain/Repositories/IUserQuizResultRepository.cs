using Testify.Domain.Entities;

namespace Testify.Domain.Repositories;

public interface IUserQuizResultRepository
{
    Task AddAsync(UserQuizResult result, CancellationToken cancellationToken = default);
    Task<UserQuizResult> GetUserQuizResultAsync(Guid quizId, Guid userId, CancellationToken cancellationToken = default);
}
