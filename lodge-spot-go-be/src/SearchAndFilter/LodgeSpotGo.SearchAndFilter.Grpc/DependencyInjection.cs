using LodgeSpotGo.SearchAndFilter.Grpc.Clients.FilterAverageGradeAccommodation;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchAccommodationClient;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchReservationClient;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.User;
using LodgeSpotGo.SearchAndFilter.Grpc.Mapper;

namespace LodgeSpotGo.SearchAndFilter.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<ISearchAccommodationClient, SearchAccommodationClient>();
        services.AddScoped<ISearchReservationClient, SearchReservationClient>();
        services.AddScoped<IFilterAverageGradeClient, FilterAverageGradeClient>();
        services.AddScoped<IUserClient, UserClient>();
        return services;
    }
}