using Interview.Application.Common;
using Interview.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Interview.Infrastructure.BackgroundJobs;

/// <summary>Hangfire job: evaluates whether the heroes assigned to a mission meet its attribute requirements.</summary>
public class MissionEvaluationJob(IAppDbContext db, IMissionNotifier notifier, ILogger<MissionEvaluationJob> logger)
{
    public async Task EvaluateAsync(Guid missionId, CancellationToken cancellationToken)
    {
        var mission = await db.Missions
            .Include(m => m.MissionHeroes)
            .ThenInclude(mh => mh.Hero)
            .FirstOrDefaultAsync(m => m.Id == missionId, cancellationToken);

        if (mission is null || mission.Status != MissionStatus.InProgress)
        {
            logger.LogWarning("Mission {MissionId} could not be evaluated: not found or not in progress.", missionId);
            return;
        }

        var heroes = mission.MissionHeroes.Select(mh => mh.Hero).ToList();

        var success = heroes.Sum(h => h.Strength) >= mission.RequiredStrength
            && heroes.Sum(h => h.Speed) >= mission.RequiredSpeed
            && heroes.Sum(h => h.Intelligence) >= mission.RequiredIntelligence
            && heroes.Sum(h => h.Durability) >= mission.RequiredDurability
            && heroes.Sum(h => h.Energy) >= mission.RequiredEnergy;

        mission.Status = success ? MissionStatus.Succeeded : MissionStatus.Failed;
        mission.CompletedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        await notifier.NotifyMissionStatusChangedAsync(mission.Id, mission.Name, mission.Status.ToString(), cancellationToken);
    }
}
