using Testify.Domain.Entities;

namespace Testify.Application.Abstractions.Repositories;

public interface IAnswerRepository
{
    Task CreateAsync(Answer entity, CancellationToken cancellationToken = default);
}
