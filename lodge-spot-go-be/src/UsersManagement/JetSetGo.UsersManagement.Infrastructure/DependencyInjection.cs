using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Application.Mapping;
using JetSetGo.UsersManagement.Infrastructure.Persistence.Configuration;
using JetSetGo.UsersManagement.Infrastructure.Persistence.Repository;
using JetSetGo.UsersManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JetSetGo.UsersManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration builderConfiguration)
    {
        services.Configure<DatabaseSettings>(settings =>
        {
            builderConfiguration.GetSection(DatabaseSettings.OptionName).Bind(settings);
        });
        
        services.AddSingleton(provider => 
            provider.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        services.AddSingleton<IHostGradeRepository, HostGradeRepository>();
        services.AddAutoMapper(typeof(MappingConfiguration));
        AddDbConfig();
        return services;
    }

    private static void AddDbConfig()
    {
        HostGradeDbConfig.Configure();
    }
    
    
}