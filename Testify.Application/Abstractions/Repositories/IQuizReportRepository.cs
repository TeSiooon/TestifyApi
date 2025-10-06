using Testify.Domain.Entities;

namespace Testify.Application.Abstractions.Repositories;

public interface IQuizReportRepository
{
    Task Create(QuizReport report, CancellationToken cancellationToken = default);
}
