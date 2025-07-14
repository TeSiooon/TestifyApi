using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Testify.Infrastructure.Quartz.Jobs;

namespace Testify.Infrastructure.Quartz;

public static class QuartzServiceCollectionExtensions
{
    public static void AddQuartzConfigurationAndJobs(this IServiceCollection services)
    {
        var jobs = new List<QuartzJobSchedule>
        {
            new QuartzJobSchedule
            {
                JobKey = JobKey.Create(nameof(LoggingBackgroundJob)),
                JobType = typeof(LoggingBackgroundJob),
                Interval = TimeSpan.FromMinutes(1)
            },
            new QuartzJobSchedule
            {
                JobKey = JobKey.Create(nameof(CloseStaleQuizzesJob)),
                JobType = typeof(CloseStaleQuizzesJob),
                Interval = TimeSpan.FromMinutes(1)
            }
        };

        services.AddQuartz(options =>
        {
            foreach (var job in jobs)
            {
                options.AddJob(job.JobType, job.JobKey)
                    .AddTrigger(trigger => trigger
                        .ForJob(job.JobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithInterval(job.Interval).RepeatForever()));
            }
        });

        services.AddQuartzHostedService();
    }
}
