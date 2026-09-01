using Interview.API.Common;
using Interview.Application.Heroes;
using Interview.Application.Heroes.List;
using MediatR;

namespace Interview.API.Features.Heroes;

public class GetHeroesEndpoint : IEndpoint
{
    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("/heroes", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var heroes = await sender.Send(new GetHeroesQuery(), cancellationToken);
            return Results.Ok(heroes);
        })
        .WithName("GetHeroes")
        .Produces<IReadOnlyList<HeroDto>>();
    }
}
