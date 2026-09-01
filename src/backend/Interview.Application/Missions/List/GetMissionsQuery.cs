using Interview.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Missions.List;

public record GetMissionsQuery : IRequest<IReadOnlyList<MissionDto>>;

public class GetMissionsHandler(IAppDbContext db) : IRequestHandler<GetMissionsQuery, IReadOnlyList<MissionDto>>
{
    public async Task<IReadOnlyList<MissionDto>> Handle(GetMissionsQuery request, CancellationToken cancellationToken)
    {
        var missions = await db.Missions
            .Include(m => m.MissionHeroes)
            .ThenInclude(mh => mh.Hero)
            .ToListAsync(cancellationToken);

        // SQLite cannot translate ORDER BY over DateTimeOffset, so order client-side instead.
        return missions.OrderByDescending(m => m.CreatedAt).Select(m => m.ToDto()).ToList();
    }
}
