using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.MessageBroker;
using JetSetGo.ReservationManagement.Infrastructure.MessageBroker.EventBus;
using JetSetGo.ReservationManagement.Infrastructure.Persistence.Configuration;
using JetSetGo.ReservationManagement.Infrastructure.Persistence.Repository;
using JetSetGo.ReservationManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JetSetGo.ReservationManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.OptionName));
        services.AddSingleton<IReservationRepository,ReservationRepository>();
        AddDbConfig();
        services.AddScoped<IEventBus, EventBus>();
        return services;
    }
    
    private static void AddDbConfig()
    {
        ReservationDbConfig.Configure();
    }
}