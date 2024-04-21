namespace Application;
using Microsoft.Extensions.DependencyInjection;
using Domain.Abstractions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<MoveEndpointListener>();
        return services;
    }
}