using Testify.Domain.Constants;
using Testify.Domain.Entities;

namespace Testify.Domain.Repositories;

public interface IQuizRepository
{
    Task Create(Quiz entity);
    Task<Quiz> GetById(Guid id);
    Task Update(Quiz entity);
    Task<(IEnumerable<Quiz>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
}
