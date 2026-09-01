using FluentValidation;
using Interview.Application.Common;
using Interview.Domain.Entities;
using MediatR;

namespace Interview.Application.Missions.Create;

public record CreateMissionCommand(
    string Name,
    string Description,
    int RequiredStrength,
    int RequiredSpeed,
    int RequiredIntelligence,
    int RequiredDurability,
    int RequiredEnergy,
    TimeSpan Duration) : IRequest<MissionDto>;

public class CreateMissionValidator : AbstractValidator<CreateMissionCommand>
{
    public CreateMissionValidator()
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

public class CreateMissionHandler(IAppDbContext db) : IRequestHandler<CreateMissionCommand, MissionDto>
{
    public async Task<MissionDto> Handle(CreateMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = new Mission
        {
            Name = request.Name,
            Description = request.Description,
            RequiredStrength = request.RequiredStrength,
            RequiredSpeed = request.RequiredSpeed,
            RequiredIntelligence = request.RequiredIntelligence,
            RequiredDurability = request.RequiredDurability,
            RequiredEnergy = request.RequiredEnergy,
            Duration = request.Duration,
        };

        db.Missions.Add(mission);
        await db.SaveChangesAsync(cancellationToken);

        return mission.ToDto();
    }
}
