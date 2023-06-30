using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neo4jClient;

namespace LodgeSpotGo.RecommodationSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration builderConfiguration)
    {
        services.Configure<DbSettings>(settings =>
        {
            builderConfiguration.GetSection(DbSettings.SectionName).Bind(settings);
        });
        services.AddSingleton(provider => 
            provider.GetRequiredService<IOptions<DbSettings>>().Value);
        // services.AddSingleton<IGraphClient>(provider =>
        // {
        //     var dbSettings = provider.GetRequiredService<DbSettings>();
        //     Console.Out.Write(dbSettings.DbName);
        //     var client = new BoltGraphClient(new Uri(dbSettings.Neo4jDb),dbSettings.DbName,dbSettings.DbPassword);
        //     client.ConnectAsync().Wait();
        //     return client;
        // });
        
        
        services.AddScoped<IRecommodationRepository, RecommodationRepository>();
        return services;
    }
}