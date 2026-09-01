using Microsoft.EntityFrameworkCore;

namespace Interview.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        await db.Database.EnsureCreatedAsync(cancellationToken);

        if (!await db.Heroes.AnyAsync(cancellationToken))
        {
            db.Heroes.AddRange(HeroSeedData.Heroes);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
