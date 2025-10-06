using Testify.Application.Abstractions.Repositories;
using Testify.Domain.Entities;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Repositories;

public class QuizReportRepository : IQuizReportRepository
{
    private readonly TestifyDbContext dbContext;

    public QuizReportRepository(TestifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Create(QuizReport entity, CancellationToken cancellationToken)
    {
        await dbContext.QuizReports.AddAsync(entity, cancellationToken);
    }
}
