using Interview.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Common;

public interface IAppDbContext
{
    DbSet<Hero> Heroes { get; }
    DbSet<Mission> Missions { get; }
    DbSet<MissionHero> MissionHeroes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
