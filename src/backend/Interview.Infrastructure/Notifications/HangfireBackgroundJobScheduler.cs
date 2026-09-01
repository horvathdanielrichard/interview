using Hangfire;
using Interview.Application.Common;
using Interview.Infrastructure.BackgroundJobs;

namespace Interview.Infrastructure.Notifications;

public class HangfireBackgroundJobScheduler(IBackgroundJobClient backgroundJobClient) : IBackgroundJobScheduler
{
    public void ScheduleMissionEvaluation(Guid missionId, TimeSpan delay)
    {
        backgroundJobClient.Schedule<MissionEvaluationJob>(job => job.EvaluateAsync(missionId, CancellationToken.None), delay);
    }
}
