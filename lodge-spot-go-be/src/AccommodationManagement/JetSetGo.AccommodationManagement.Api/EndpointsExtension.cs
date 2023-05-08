using JetSetGo.AccommodationManagement.Api.Endpoints;

namespace JetSetGo.AccommodationManagement.Api;

public static class EndpointsExtension
{
    public static void MapEndpoints(this WebApplication application)
    {
        application.MapAccommodationEndpoints();
        application.MapBaseEndpoint();
    }
}