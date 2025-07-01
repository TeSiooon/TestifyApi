using Testify.Domain.Entities;

namespace Testify.Domain.Repositories;

public interface IAnswerRepository
{
    Task CreateAsync(Answer entity, CancellationToken cancellationToken = default);
}
