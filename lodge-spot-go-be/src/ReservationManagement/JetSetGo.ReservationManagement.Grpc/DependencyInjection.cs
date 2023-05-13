using JetSetGo.ReservationManagement.Grpc.Mapping;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;

namespace JetSetGo.ReservationManagement.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<IMapToGrpcResponse, MapToGrpcResponse>();
        return services;
    }
}