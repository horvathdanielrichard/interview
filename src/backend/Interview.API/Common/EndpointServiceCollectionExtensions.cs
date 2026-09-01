using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Interview.API.Common;

public static class EndpointServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var descriptors = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && typeof(IEndpoint).IsAssignableFrom(type))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type.AsType()));

        services.TryAddEnumerable(descriptors);

        return services;
    }
}
