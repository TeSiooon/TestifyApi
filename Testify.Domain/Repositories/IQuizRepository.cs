using Testify.Domain.Entities;

namespace Testify.Domain.Repositories;

public interface IQuizRepository
{
    Task Create(Quiz entity);
    Task<Quiz> GetById(Guid id);
    Task Update(Quiz entity);
}
