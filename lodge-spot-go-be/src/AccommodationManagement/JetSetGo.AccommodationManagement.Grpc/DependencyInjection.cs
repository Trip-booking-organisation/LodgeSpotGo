using JetSetGo.AccommodationManagement.Grpc.Clients;
using JetSetGo.AccommodationManagement.Grpc.Mapping;
using JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;

namespace JetSetGo.AccommodationManagement.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services,ConfigurationManager builderConfiguration)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<IMappingToGrpcResponse, MappingToGrpcResponse>();
        services.AddScoped<IReservationClient, ReservationClient>();
        //services.AddCorsPolicy(builderConfiguration);
        return services;
    }
    private static void AddCorsPolicy(this IServiceCollection services, IConfiguration builderConfiguration)
    {
        var corsSection = builderConfiguration.GetSection("Cors");
        var policyName = corsSection.GetSection("PolicyName").Value!;
        var origins = corsSection.GetSection("Origins").Value!.Split(";");
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, 
                builder => builder
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
}