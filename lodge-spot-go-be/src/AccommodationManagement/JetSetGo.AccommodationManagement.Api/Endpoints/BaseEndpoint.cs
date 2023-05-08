namespace JetSetGo.AccommodationManagement.Api.Endpoints;

public  static class BaseEndpoint
{
    public static void MapBaseEndpoint(this WebApplication app)
    {
        app.MapGet("/", () => 
            Results.Content("Welcome to Lodge Set Go!")
            );
    }
}