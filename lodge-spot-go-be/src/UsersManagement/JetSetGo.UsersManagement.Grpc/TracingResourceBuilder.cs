using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Endpoints;
using JetSetGo.UsersManagement.Grpc.Services;
using OpenTelemetry.Resources;

namespace JetSetGo.UsersManagement.Grpc;

public static class TracingResourceBuilder
{
    public static ResourceBuilder GetUserServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserEndpoints.UserService).AddTelemetrySdk();
    }
}