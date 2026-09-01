using FluentValidation;
using Interview.Application.Common;
using Interview.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Missions.Update;

public record UpdateMissionCommand(
    Guid Id,
    string Name,
    string Description,
    int RequiredStrength,
    int RequiredSpeed,
    int RequiredIntelligence,
    int RequiredDurability,
    int RequiredEnergy,
    TimeSpan Duration) : IRequest<MissionDto>;

public class UpdateMissionValidator : AbstractValidator<UpdateMissionCommand>
{
    public UpdateMissionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Duration).GreaterThan(TimeSpan.Zero);
        RuleFor(x => x.RequiredStrength).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RequiredSpeed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RequiredIntelligence).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RequiredDurability).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RequiredEnergy).GreaterThanOrEqualTo(0);
    }
}

public class UpdateMissionHandler(IAppDbContext db) : IRequestHandler<UpdateMissionCommand, MissionDto>
{
    public async Task<MissionDto> Handle(UpdateMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = await db.Missions
            .Include(m => m.MissionHeroes)
            .ThenInclude(mh => mh.Hero)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Mission '{request.Id}' was not found.");

        if (mission.Status != MissionStatus.Pending)
        {
            throw new InvalidOperationException("Only pending missions can be modified.");
        }

        mission.Name = request.Name;
        mission.Description = request.Description;
        mission.RequiredStrength = request.RequiredStrength;
        mission.RequiredSpeed = request.RequiredSpeed;
        mission.RequiredIntelligence = request.RequiredIntelligence;
        mission.RequiredDurability = request.RequiredDurability;
        mission.RequiredEnergy = request.RequiredEnergy;
        mission.Duration = request.Duration;

        await db.SaveChangesAsync(cancellationToken);

        return mission.ToDto();
    }
}
