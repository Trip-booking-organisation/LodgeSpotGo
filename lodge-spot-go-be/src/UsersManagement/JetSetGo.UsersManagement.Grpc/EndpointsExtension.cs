using JetSetGo.UsersManagement.Grpc.Endpoints;

namespace JetSetGo.UsersManagement.Grpc;

public static class EndpointsExtension
{
  public static void MapEndpoints(this WebApplication webApplication)
  {
    webApplication.MapUserEndpoints();
  }
}