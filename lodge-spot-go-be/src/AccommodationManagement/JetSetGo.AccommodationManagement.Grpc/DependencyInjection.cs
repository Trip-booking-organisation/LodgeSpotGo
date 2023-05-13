using JetSetGo.AccommodationManagement.Grpc.Mapping;

namespace JetSetGo.AccommodationManagement.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddAPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<IMappingToGrpcResponse, MappingToGrpcResponse>();
        return services;
    }
}