using LodgeSpotGo.SearchAndFilter.Grpc.Services;
using OpenTelemetry.Resources;

namespace LodgeSpotGo.SearchAndFilter.Grpc;

public static class TracingResourceBuilder
{
    public static ResourceBuilder SearchAndFilterServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(SearchAndFilterService.ServiceName);
    }
  
}