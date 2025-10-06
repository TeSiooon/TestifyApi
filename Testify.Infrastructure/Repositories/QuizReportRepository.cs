using Microsoft.EntityFrameworkCore;
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

    public async Task<QuizReport> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var report = await dbContext.QuizReports.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException("Quiz report not found");

        return report;
    }
}
