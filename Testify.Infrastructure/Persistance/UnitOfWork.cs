using Testify.Application.Common;

namespace Testify.Infrastructure.Persistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly TestifyDbContext dbContext;

    public UnitOfWork(TestifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
