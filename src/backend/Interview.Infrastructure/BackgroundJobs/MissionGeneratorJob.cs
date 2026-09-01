using Interview.Application.Common;
using Interview.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Interview.Infrastructure.BackgroundJobs;

/// <summary>Hangfire recurring job: periodically generates a new mission with randomized requirements.</summary>
public class MissionGeneratorJob(IAppDbContext db, ILogger<MissionGeneratorJob> logger)
{
    private static readonly string[] Adjectives =
    [
        "Silent", "Crimson", "Shattered", "Hidden", "Rising", "Frozen", "Burning", "Lost", "Final", "Ancient",
    ];

    private static readonly string[] Objectives =
    [
        "Convoy", "Outpost", "Signal", "Vault", "Bridge", "Reactor", "Harbor", "Tower", "Archive", "Frontier",
    ];

    private static readonly string[] DescriptionTemplates =
    [
        "Intercept hostile activity near the {0} before reinforcements arrive.",
        "Secure the {0} and evacuate any survivors in the area.",
        "Neutralize the threat surrounding the {0} without civilian casualties.",
        "Investigate strange readings coming from the {0} and report back.",
        "Defend the {0} against an incoming wave of attackers.",
    ];

    public async Task GenerateAsync()
    {
        var random = Random.Shared;
        var objective = Objectives[random.Next(Objectives.Length)];
        var name = $"Operation {Adjectives[random.Next(Adjectives.Length)]} {objective}";
        var description = string.Format(DescriptionTemplates[random.Next(DescriptionTemplates.Length)], objective);

        var mission = new Mission
        {
            Name = name,
            Description = description,
            RequiredStrength = RandomRequirement(random),
            RequiredSpeed = RandomRequirement(random),
            RequiredIntelligence = RandomRequirement(random),
            RequiredDurability = RandomRequirement(random),
            RequiredEnergy = RandomRequirement(random),
            Duration = TimeSpan.FromMinutes(random.Next(1, 6)),
        };

        db.Missions.Add(mission);
        await db.SaveChangesAsync(CancellationToken.None);

        logger.LogInformation("Generated new mission {MissionName} ({MissionId}).", mission.Name, mission.Id);
    }

    private static int RandomRequirement(Random random) => random.Next(20, 121);
}
