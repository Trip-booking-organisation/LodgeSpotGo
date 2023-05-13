using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchAccommodationClient;
using LodgeSpotGo.SearchAndFilter.Grpc.Mapper;

namespace LodgeSpotGo.SearchAndFilter.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddAPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<ISearchAccommodationClient, SearchAccommodationClient>();
        return services;
    }
}