using Microsoft.EntityFrameworkCore;
using Testify.Application.Abstractions.Repositories;
using Testify.Domain.Entities;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly TestifyDbContext dbContext;
    public QuestionRepository(TestifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task CreateAsync(Question entity, CancellationToken cancellationToken)
    {
        dbContext.Questions.AddAsync(entity, cancellationToken);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var question = await dbContext.Questions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException();
        dbContext.Questions.Remove(question);
    }

    public async Task<Question> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var question = await dbContext.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException();

        return question;
    }

    public Task UpdateAsync(Question entity, CancellationToken cancellationToken = default)
    {
        dbContext.Questions.Update(entity);
        return Task.CompletedTask;
    }
}
