using Interview.API.Common;
using Interview.Application.Missions;
using Interview.Application.Missions.Create;
using MediatR;

namespace Interview.API.Features.Missions;

public record CreateMissionRequest(
    string Name,
    string Description,
    int RequiredStrength,
    int RequiredSpeed,
    int RequiredIntelligence,
    int RequiredDurability,
    int RequiredEnergy,
    TimeSpan Duration);

public class CreateMissionEndpoint : IEndpoint
{
    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/missions", async (CreateMissionRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateMissionCommand(
                request.Name,
                request.Description,
                request.RequiredStrength,
                request.RequiredSpeed,
                request.RequiredIntelligence,
                request.RequiredDurability,
                request.RequiredEnergy,
                request.Duration);

            var mission = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/missions/{mission.Id}", mission);
        })
        .WithName("CreateMission")
        .Produces<MissionDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
