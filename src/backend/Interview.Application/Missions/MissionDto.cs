namespace Interview.Application.Missions;

public record MissionDto(
    Guid Id,
    string Name,
    string Description,
    int RequiredStrength,
    int RequiredSpeed,
    int RequiredIntelligence,
    int RequiredDurability,
    int RequiredEnergy,
    TimeSpan Duration,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt,
    IReadOnlyList<MissionHeroDto> AssignedHeroes);

public record MissionHeroDto(Guid HeroId, string Name);
