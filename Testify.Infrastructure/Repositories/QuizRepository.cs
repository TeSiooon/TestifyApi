using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Testify.Domain.Constants;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly TestifyDbContext dbContext;
    public QuizRepository(TestifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task Create(Quiz entity)
    {
        await dbContext.Quizzes.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Quiz>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery =  dbContext.Quizzes
            .Where(q => searchPhraseLower == null
            || (q.Title.ToLower().Contains(searchPhraseLower)));

        var totalCount = await baseQuery.CountAsync();
        if(sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Quiz, object>>>
            {
                { nameof(Quiz.Title), q => q.Title },
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var quizzes = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (quizzes, totalCount);
    }

    public async Task<Quiz> GetById(Guid id)
    {
        var result = await dbContext.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(question => question.Answers)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException();

        return result;
    }

    public Task Update(Quiz entity)
    {
        throw new NotImplementedException();
    }
}
