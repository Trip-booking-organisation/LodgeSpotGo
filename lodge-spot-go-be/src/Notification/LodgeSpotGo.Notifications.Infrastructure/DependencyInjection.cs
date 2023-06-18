using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Infrastructure.Persistence.Configuration;
using LodgeSpotGo.Notifications.Infrastructure.Persistence.Repository;
using LodgeSpotGo.Notifications.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LodgeSpotGo.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.Configure<DbSettings>(settings =>
        {
            configuration.GetSection(DbSettings.OptionName).Bind(settings);
        });
        services.AddSingleton(provider => 
            provider.GetRequiredService<IOptions<DbSettings>>().Value);
        NotificationsDbConfiguration.Configure();
        services.AddScoped<IHostNotificationRepository, HostNotificationRepository>();
        services.AddScoped<IGuestNotificationRepository, GuestNotificationsRepository>();
        return services;
    }
}