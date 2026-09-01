using Interview.API.Common;
using Interview.Application.Missions;
using Interview.Application.Missions.Get;
using MediatR;

namespace Interview.API.Features.Missions;

public class GetMissionByIdEndpoint : IEndpoint
{
    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/missions/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var mission = await sender.Send(new GetMissionByIdQuery(id), cancellationToken);
            return Results.Ok(mission);
        })
        .WithName("GetMissionById")
        .Produces<MissionDto>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
