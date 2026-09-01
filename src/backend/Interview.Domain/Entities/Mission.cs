using Interview.Domain.Enums;

namespace Interview.Domain.Entities;

public class Mission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int RequiredStrength { get; set; }
    public int RequiredSpeed { get; set; }
    public int RequiredIntelligence { get; set; }
    public int RequiredDurability { get; set; }
    public int RequiredEnergy { get; set; }

    /// <summary>How long the mission takes to complete once heroes are assigned.</summary>
    public TimeSpan Duration { get; set; }

    public MissionStatus Status { get; set; } = MissionStatus.Pending;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }

    public ICollection<MissionHero> MissionHeroes { get; set; } = new List<MissionHero>();
}
