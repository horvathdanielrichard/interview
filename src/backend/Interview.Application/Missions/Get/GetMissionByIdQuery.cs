using Interview.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Missions.Get;

public record GetMissionByIdQuery(Guid Id) : IRequest<MissionDto>;

public class GetMissionByIdHandler(IAppDbContext db) : IRequestHandler<GetMissionByIdQuery, MissionDto>
{
    public async Task<MissionDto> Handle(GetMissionByIdQuery request, CancellationToken cancellationToken)
    {
        var mission = await db.Missions
            .Include(m => m.MissionHeroes)
            .ThenInclude(mh => mh.Hero)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Mission '{request.Id}' was not found.");

        return mission.ToDto();
    }
}
