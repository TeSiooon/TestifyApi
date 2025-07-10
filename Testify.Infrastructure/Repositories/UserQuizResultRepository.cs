using Microsoft.EntityFrameworkCore;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Repositories;

public class UserQuizResultRepository : IUserQuizResultRepository
{
    private readonly TestifyDbContext dbContext;

    public UserQuizResultRepository(TestifyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(UserQuizResult result, CancellationToken cancellationToken = default)
    {
        await dbContext.UserQuizResults.AddAsync(result, cancellationToken);
    }

    public async Task<UserQuizResult> GetUserQuizResultAsync(Guid quizId, Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await dbContext.UserQuizResults
            .FirstOrDefaultAsync(x => x.QuizId == quizId && x.UserId == userId, cancellationToken);

        return result;
    }
}
