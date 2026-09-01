using Interview.Application.Common;
using Interview.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Missions.Delete;

public record DeleteMissionCommand(Guid Id) : IRequest;

public class DeleteMissionHandler(IAppDbContext db) : IRequestHandler<DeleteMissionCommand>
{
    public async Task Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = await db.Missions.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Mission '{request.Id}' was not found.");

        if (mission.Status == MissionStatus.InProgress)
        {
            throw new InvalidOperationException("A mission that is in progress cannot be deleted.");
        }

        db.Missions.Remove(mission);
        await db.SaveChangesAsync(cancellationToken);
    }
}
