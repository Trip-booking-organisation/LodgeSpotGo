using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Endpoints;
using JetSetGo.UsersManagement.Grpc.Services;
using OpenTelemetry.Resources;

namespace JetSetGo.UsersManagement.Grpc;

public static class TracingResourceBuilder
{
    public static ResourceBuilder GetUserServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserEndpoints.GetUserActivity.Name).AddTelemetrySdk();
    }
    public static ResourceBuilder BuyTicketsServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserEndpoints.BuyTicketsActivity.Name);
    }
    public static ResourceBuilder DeleteGradeServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserEndpoints.DeleteGradeActivity.Name);
    }
    public static ResourceBuilder GetOutstandingHostServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserEndpoints.GetOutstandingHostActivity.Name).AddTelemetrySdk();
    }
    public static ResourceBuilder UpdateGradeServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserEndpoints.UpdateGradeActivity.Name);
    }
    public static ResourceBuilder GetGradesByHostServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserEndpoints.GetGradesByHostServiceActivity.Name);
    }
    public static ResourceBuilder FiletServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(FilterService.Activity.Name);
    }
    public static ResourceBuilder UserServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(GetUserService.Activity.Name);
    }
}