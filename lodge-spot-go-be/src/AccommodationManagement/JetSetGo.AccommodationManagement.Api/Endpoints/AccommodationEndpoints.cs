using MediatR;

namespace JetSetGo.AccommodationManagement.Api.Endpoints;

public static class AccommodationEndpoints
{
    public static void MapAccommodationEndpoints(this WebApplication app)
    {
        app.MapGet("ap1/v1/accommodation", GetAllAccommodations);
    }

    private static async Task<IResult> GetAllAccommodations(ISender sender)
    {
        await Task.CompletedTask;
        return Results.Ok();
    }
}