using Testify.Domain.Entities;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly TestifyDbContext dbContext;

    public AnswerRepository(TestifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task CreateAsync(Answer entity, CancellationToken cancellationToken)
    {
        dbContext.Answers.AddAsync(entity, cancellationToken);
        return Task.CompletedTask;
    }
}
