using Microsoft.Extensions.Logging;
using Quartz;
using Testify.Application.Common;
using Testify.Domain.Repositories;

namespace Testify.Infrastructure.Quartz.Jobs;

[DisallowConcurrentExecution]
// This job closes stale quiz attempts that have exceeded their time limit
public class CloseStaleQuizzesJob : IJob
{
    private readonly IUserQuizAttemptRepository userQuizAttemptRepository;
    private readonly ILogger<CloseStaleQuizzesJob> logger;
    private readonly IUnitOfWork unitOfWork;

    public CloseStaleQuizzesJob(IUserQuizAttemptRepository userQuizAttemptRepository, ILogger<CloseStaleQuizzesJob> logger,
        IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.userQuizAttemptRepository = userQuizAttemptRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTime.UtcNow;

        var attempts = await userQuizAttemptRepository.GetAllOpenAttemptsWithQuizAsync();
        if (attempts is null)
        {
            logger.LogInformation("No open quiz attempts found to close.");
            return;
        }

        foreach (var attempt in attempts)
        {
            var timeLimit = attempt.Quiz.TimeLimit ?? TimeSpan.FromHours(24);
            var deadline = attempt.StartedAt.Add(timeLimit);

            if (now < deadline)
                continue;

            attempt.Finish();

            var correctAnswers = attempt.Answers.Count(a => a.SelectedAnswer.IsCorrect);
        }
        await unitOfWork.SaveChangesAsync();
    }
}
