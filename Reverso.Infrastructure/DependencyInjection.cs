using Microsoft.Extensions.DependencyInjection;
using Application;

namespace Reverso.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register all infrastructure services here
        services.AddScoped<IUserStorage, UserStorage>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ILobbyStorage, LobbyStorage>();

        return services;
    }
}