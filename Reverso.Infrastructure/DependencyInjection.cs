using Microsoft.Extensions.DependencyInjection;
using Application;
using Reverso.Domain.Web;

namespace Reverso.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string mongoConnectionString, string dbName)
    {
        services.AddSingleton<MongoDbContext>(provider => new MongoDbContext(mongoConnectionString, dbName));
        services.AddScoped<IUserStorage, MongoUserStorage>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ILobbyStorage, LobbyStorage>();
        return services;
    }
}