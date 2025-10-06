using Testify.Domain.Entities;

namespace Testify.Application.Abstractions.Repositories;

public interface IQuizReportRepository
{
    Task Create(QuizReport report, CancellationToken cancellationToken = default);
    Task<QuizReport> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
