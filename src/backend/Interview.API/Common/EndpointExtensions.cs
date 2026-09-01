public static class EndpointExtensions
{
    public static void MapEndpoints(this WebApplication app, string prefix = "/api")
    {
        var routeGroupBuilder = app.MapGroup(prefix).WithOpenApi();
        foreach (var endpoint in app.Services.GetServices<IEndpoint>())
        {
            endpoint.MapEndpoints(routeGroupBuilder);
        }
    }
}