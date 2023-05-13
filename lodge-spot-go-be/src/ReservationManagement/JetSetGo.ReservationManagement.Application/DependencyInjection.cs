using JetSetGo.ReservationManagement.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace JetSetGo.ReservationManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
        return services;
    }
}