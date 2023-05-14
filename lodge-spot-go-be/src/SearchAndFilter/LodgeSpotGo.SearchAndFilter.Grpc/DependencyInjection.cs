using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchAccommodationClient;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchReservationClient;
using LodgeSpotGo.SearchAndFilter.Grpc.Mapper;

namespace LodgeSpotGo.SearchAndFilter.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<ISearchAccommodationClient, SearchAccommodationClient>();
        services.AddScoped<ISearchReservationClient, SearchReservationClient>();
        return services;
    }
}