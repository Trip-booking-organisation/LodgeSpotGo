using JetSetGo.ReservationManagement.Grpc.Mapping;

namespace JetSetGo.ReservationManagement.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        return services;
    }
}