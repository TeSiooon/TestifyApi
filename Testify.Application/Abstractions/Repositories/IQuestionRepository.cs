using Testify.Domain.Entities;

namespace Testify.Application.Abstractions.Repositories;

public interface IQuestionRepository
{
    Task CreateAsync(Question entity, CancellationToken cancellationToken = default);
    Task<Question> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Question entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
