using Microsoft.EntityFrameworkCore;
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

    public async Task<Quiz> GetById(Guid id)
    {
        var result = await dbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException();

        return result;
    }

    public Task Update(Quiz entity)
    {
        throw new NotImplementedException();
    }
}
