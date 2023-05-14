using JetSetGo.UsersManagement.Api.Endpoints;

namespace JetSetGo.UsersManagement.Api;

public static class EndpointsExtension
{
  public static void MapEndpoints(this WebApplication webApplication)
  {
    webApplication.MapUserEndpoints();
  }
}