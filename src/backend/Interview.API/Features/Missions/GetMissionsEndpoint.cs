using Interview.API.Common;
using Interview.Application.Missions;
using Interview.Application.Missions.List;
using MediatR;

namespace Interview.API.Features.Missions;

public class GetMissionsEndpoint : IEndpoint
{
    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/missions", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var missions = await sender.Send(new GetMissionsQuery(), cancellationToken);
            return Results.Ok(missions);
        })
        .WithName("GetMissions")
        .Produces<IReadOnlyList<MissionDto>>();
    }
}
