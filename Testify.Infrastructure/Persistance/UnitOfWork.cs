using Microsoft.EntityFrameworkCore;
using Testify.Application.Common;
using Testify.Common.Entities;

namespace Testify.Infrastructure.Persistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly TestifyDbContext dbContext;
    private readonly ICurrentUserService currentUserService;

    public UnitOfWork(TestifyDbContext dbContext, ICurrentUserService currentUserService)
    {
        this.dbContext = dbContext;
        this.currentUserService = currentUserService;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = dbContext.ChangeTracker.Entries<AuditableEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var userId = currentUserService.UserId;

        var nov = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedAt = nov;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedAt = nov;
            }
        }

        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
