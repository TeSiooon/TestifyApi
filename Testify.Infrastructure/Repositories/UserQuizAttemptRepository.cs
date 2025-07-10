using Microsoft.EntityFrameworkCore;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Repositories;

public class UserQuizAttemptRepository : IUserQuizAttemptRepository
{
    private readonly TestifyDbContext dbContext;

    public UserQuizAttemptRepository(TestifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task AddAsync(UserQuizAttempt attempt, CancellationToken cancellationToken)
    {
        await dbContext.UserQuizAttempts.AddAsync(attempt);
    }

    public async Task<int> CountAttemptsAsync(Guid userId, Guid quizId, CancellationToken cancellationToken)
    {
        return await dbContext.UserQuizAttempts
            .Where(a => a.UserId == userId && a.QuizId == quizId)
            .CountAsync(cancellationToken);
    }

    public async Task<UserQuizAttempt?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.UserQuizAttempts
            .Include(ua => ua.Answers)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken) ?? throw new KeyNotFoundException();
    }

    public async Task<UserQuizAttempt?> GetWithAnswersAsync(Guid attemptId, CancellationToken cancellationToken)
    {
        return await dbContext.UserQuizAttempts
            .Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id == attemptId, cancellationToken);
    }

    public async Task<UserQuizAttempt?> GetWithQuizAndAnswersAsync(Guid attemptId, CancellationToken cancellationToken)
    {
        return await dbContext.UserQuizAttempts
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id == attemptId, cancellationToken);
    }

    public Task AddAnswerAsync(UserAnswer answer, CancellationToken ct)
    {
        return dbContext.UserAnswers.AddAsync(answer, ct).AsTask();
    }
}
