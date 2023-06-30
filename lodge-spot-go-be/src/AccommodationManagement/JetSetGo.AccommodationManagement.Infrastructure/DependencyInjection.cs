using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Application.Mapping;
using JetSetGo.AccommodationManagement.Application.MessageBroker;
using JetSetGo.AccommodationManagement.Infrastructure.MessageBroker.EventBus;
using JetSetGo.AccommodationManagement.Infrastructure.Persistence.Configuration;
using JetSetGo.AccommodationManagement.Infrastructure.Persistence.Repository;
using JetSetGo.AccommodationManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JetSetGo.AccommodationManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,ConfigurationManager builderConfiguration)
    {
        services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.OptionName));
        services.AddSingleton<IAccommodationRepository,AccommodationRepository>();
        services.AddSingleton<IGradeRepository, GradeRepository>();
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<IEventBus, EventBus>();
        AddDbConfig();
        return services;
    }

    private static void AddDbConfig()
    {
        AccommodationDbConfig.Configure();
        GradeDbConfig.Configure();
    }
}