using Interview.API.Common;
using Interview.Application.Missions.Delete;
using MediatR;

namespace Interview.API.Features.Missions;

public class DeleteMissionEndpoint : IEndpoint
{
    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapDelete("/missions/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteMissionCommand(id), cancellationToken);
            return Results.NoContent();
        })
        .WithName("DeleteMission")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
