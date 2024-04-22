using Microsoft.Extensions.DependencyInjection;
using Application;
using Reverso.Domain.Web;

namespace Reverso.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string mongoConnectionString, string dbName)
    {
        services.AddSingleton<MongoDbContext>(provider => new MongoDbContext(mongoConnectionString, dbName));
        services.AddSingleton<IUserStorage, MongoUserStorage>();
        services.AddSingleton<ILobbyStorage, LobbyStorage>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}