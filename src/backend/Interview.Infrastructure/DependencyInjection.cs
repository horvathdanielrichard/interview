using Hangfire;
using Interview.Application.Common;
using Interview.Infrastructure.BackgroundJobs;
using Interview.Infrastructure.Notifications;
using Interview.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Interview.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Named shared-cache SQLite in-memory database. Data lives only as long as the process runs - by
    /// design, nothing is persisted to disk so every container start begins from a clean, re-seeded database.
    /// </summary>
    private const string SqliteConnectionString = "Data Source=interview-db;Mode=Memory;Cache=Shared";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Keeps the shared in-memory database alive for the lifetime of the app; disposed by the DI container on shutdown.
        services.AddSingleton(_ =>
        {
            var keepAliveConnection = new SqliteConnection(SqliteConnectionString);
            keepAliveConnection.Open();
            return keepAliveConnection;
        });

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(SqliteConnectionString));
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddSignalR();
        services.AddScoped<IMissionNotifier, SignalRMissionNotifier>();
        services.AddScoped<IBackgroundJobScheduler, HangfireBackgroundJobScheduler>();
        services.AddScoped<MissionEvaluationJob>();
        services.AddScoped<MissionGeneratorJob>();

        services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseInMemoryStorage());
        services.AddHangfireServer();

        return services;
    }
}
