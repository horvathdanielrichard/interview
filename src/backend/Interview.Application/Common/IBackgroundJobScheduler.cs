namespace Interview.Application.Common;

/// <summary>Enqueues delayed and recurring background work. Implemented in Infrastructure using Hangfire.</summary>
public interface IBackgroundJobScheduler
{
    void ScheduleMissionEvaluation(Guid missionId, TimeSpan delay);
}
