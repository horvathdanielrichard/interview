namespace Interview.Domain.Entities;

/// <summary>Join entity recording which heroes were assigned to a mission.</summary>
public class MissionHero
{
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;

    public Guid HeroId { get; set; }
    public Hero Hero { get; set; } = null!;

    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;
}
