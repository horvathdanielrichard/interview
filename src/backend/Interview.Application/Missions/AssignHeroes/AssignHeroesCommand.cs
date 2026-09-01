using FluentValidation;
using Interview.Application.Common;
using Interview.Domain.Entities;
using Interview.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Missions.AssignHeroes;

public record AssignHeroesCommand(Guid MissionId, IReadOnlyList<Guid> HeroIds) : IRequest<MissionDto>;

public class AssignHeroesValidator : AbstractValidator<AssignHeroesCommand>
{
    public AssignHeroesValidator()
    {
        RuleFor(x => x.HeroIds).NotEmpty().WithMessage("At least one hero must be assigned to a mission.");
        RuleForEach(x => x.HeroIds).NotEmpty();
    }
}

public class AssignHeroesHandler(IAppDbContext db, IBackgroundJobScheduler jobScheduler)
    : IRequestHandler<AssignHeroesCommand, MissionDto>
{
    public async Task<MissionDto> Handle(AssignHeroesCommand request, CancellationToken cancellationToken)
    {
        var mission = await db.Missions
            .Include(m => m.MissionHeroes)
            .ThenInclude(mh => mh.Hero)
            .FirstOrDefaultAsync(m => m.Id == request.MissionId, cancellationToken)
            ?? throw new NotFoundException($"Mission '{request.MissionId}' was not found.");

        if (mission.Status != MissionStatus.Pending)
        {
            throw new InvalidOperationException("Heroes can only be assigned to a pending mission.");
        }

        var distinctHeroIds = request.HeroIds.Distinct().ToList();
        var heroes = await db.Heroes
            .Where(h => distinctHeroIds.Contains(h.Id))
            .ToListAsync(cancellationToken);

        if (heroes.Count != distinctHeroIds.Count)
        {
            throw new NotFoundException("One or more heroes could not be found.");
        }

        var now = DateTimeOffset.UtcNow;
        foreach (var hero in heroes)
        {
            mission.MissionHeroes.Add(new MissionHero { MissionId = mission.Id, HeroId = hero.Id, AssignedAt = now });
        }

        mission.Status = MissionStatus.InProgress;
        mission.StartedAt = now;

        await db.SaveChangesAsync(cancellationToken);

        jobScheduler.ScheduleMissionEvaluation(mission.Id, mission.Duration);

        return mission.ToDto();
    }
}
