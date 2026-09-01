using Interview.API.Common;
using Interview.Application.Missions;
using Interview.Application.Missions.AssignHeroes;
using MediatR;

namespace Interview.API.Features.Missions;

public record AssignHeroesRequest(IReadOnlyList<Guid> HeroIds);

public class AssignHeroesEndpoint : IEndpoint
{
    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/missions/{id:guid}/assign", async (Guid id, AssignHeroesRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var mission = await sender.Send(new AssignHeroesCommand(id, request.HeroIds), cancellationToken);
            return Results.Ok(mission);
        })
        .WithName("AssignHeroesToMission")
        .Produces<MissionDto>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
