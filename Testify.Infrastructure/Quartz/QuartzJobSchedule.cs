using Quartz;

namespace Testify.Infrastructure.Quartz;

public class QuartzJobSchedule
{
    public JobKey JobKey { get; set; }
    public Type JobType { get; set; }
    public TimeSpan Interval { get; set; }
}
