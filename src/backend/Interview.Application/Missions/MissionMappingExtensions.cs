using Interview.Domain.Entities;

namespace Interview.Application.Missions;

public static class MissionMappingExtensions
{
    public static MissionDto ToDto(this Mission mission) => new(
        mission.Id,
        mission.Name,
        mission.Description,
        mission.RequiredStrength,
        mission.RequiredSpeed,
        mission.RequiredIntelligence,
        mission.RequiredDurability,
        mission.RequiredEnergy,
        mission.Duration,
        mission.Status.ToString(),
        mission.CreatedAt,
        mission.StartedAt,
        mission.CompletedAt,
        mission.MissionHeroes
            .Select(mh => new MissionHeroDto(mh.HeroId, mh.Hero.Name))
            .ToList());
}
