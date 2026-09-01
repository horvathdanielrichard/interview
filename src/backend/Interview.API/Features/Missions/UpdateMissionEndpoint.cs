using Interview.API.Common;
using Interview.Application.Missions;
using Interview.Application.Missions.Update;
using MediatR;

namespace Interview.API.Features.Missions;

public record UpdateMissionRequest(
    string Name,
    string Description,
    int RequiredStrength,
    int RequiredSpeed,
    int RequiredIntelligence,
    int RequiredDurability,
    int RequiredEnergy,
    TimeSpan Duration);

public class UpdateMissionEndpoint : IEndpoint
{
    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPut("/missions/{id:guid}", async (Guid id, UpdateMissionRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateMissionCommand(
                id,
                request.Name,
                request.Description,
                request.RequiredStrength,
                request.RequiredSpeed,
                request.RequiredIntelligence,
                request.RequiredDurability,
                request.RequiredEnergy,
                request.Duration);

            var mission = await sender.Send(command, cancellationToken);
            return Results.Ok(mission);
        })
        .WithName("UpdateMission")
        .Produces<MissionDto>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
