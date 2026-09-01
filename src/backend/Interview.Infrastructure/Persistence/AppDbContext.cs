using Interview.Application.Common;
using Interview.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interview.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Hero> Heroes => Set<Hero>();
    public DbSet<Mission> Missions => Set<Mission>();
    public DbSet<MissionHero> MissionHeroes => Set<MissionHero>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hero>(builder =>
        {
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Name).IsRequired().HasMaxLength(200);
            builder.Property(h => h.AlterEgo).HasMaxLength(200);
        });

        modelBuilder.Entity<Mission>(builder =>
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Description).IsRequired().HasMaxLength(2000);
            builder.Property(m => m.Status).HasConversion<string>().HasMaxLength(20);
        });

        modelBuilder.Entity<MissionHero>(builder =>
        {
            builder.HasKey(mh => new { mh.MissionId, mh.HeroId });

            builder.HasOne(mh => mh.Mission)
                .WithMany(m => m.MissionHeroes)
                .HasForeignKey(mh => mh.MissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(mh => mh.Hero)
                .WithMany(h => h.MissionHeroes)
                .HasForeignKey(mh => mh.HeroId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
