using System.Reflection;
using Hangfire;
using Interview.API.Common;
using Interview.Application;
using Interview.Infrastructure;
using Interview.Infrastructure.BackgroundJobs;
using Interview.Infrastructure.Notifications;
using Interview.Infrastructure.Persistence;

const string CorsPolicyName = "Frontend";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ?? ["http://localhost:4200"];

    options.AddPolicy(CorsPolicyName, policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

var app = builder.Build();

// Forces the shared-cache SQLite in-memory keep-alive connection to open now, before anything queries the
// database. Without this, the connection is never resolved and the in-memory database is dropped as soon as
// the first EF Core operation closes its own transient connection.
app.Services.GetRequiredService<Microsoft.Data.Sqlite.SqliteConnection>();

app.UseExceptionHandler();
app.UseCors(CorsPolicyName);

app.MapHub<NotificationHub>("/hubs/notifications");
app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/hangfire");
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.InitializeAsync(db);
}

RecurringJob.AddOrUpdate<MissionGeneratorJob>(
    "generate-random-mission",
    job => job.GenerateAsync(),
    "*/2 * * * *");

app.Run();
