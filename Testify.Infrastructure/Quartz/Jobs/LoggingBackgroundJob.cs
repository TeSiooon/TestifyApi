using Microsoft.Extensions.Logging;
using Quartz;

namespace Testify.Infrastructure.Quartz.Jobs;

[DisallowConcurrentExecution]
// for test purposes only
public class LoggingBackgroundJob : IJob
{
    private readonly ILogger<LoggingBackgroundJob> logger;
    public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger)
    {
        this.logger = logger;
    }
    public Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Executing LoggingBackgroundJob at {Time}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}
