using Interview.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Heroes.List;

public record GetHeroesQuery : IRequest<IReadOnlyList<HeroDto>>;

public class GetHeroesHandler(IAppDbContext db) : IRequestHandler<GetHeroesQuery, IReadOnlyList<HeroDto>>
{
    public async Task<IReadOnlyList<HeroDto>> Handle(GetHeroesQuery request, CancellationToken cancellationToken)
    {
        return await db.Heroes
            .OrderBy(h => h.Name)
            .Select(h => new HeroDto(h.Id, h.Name, h.AlterEgo, h.Strength, h.Speed, h.Intelligence, h.Durability, h.Energy))
            .ToListAsync(cancellationToken);
    }
}
