using JetSetGo.RecommodationSystem.Grpc.Services;
using OpenTelemetry.Resources;

namespace JetSetGo.RecommodationSystem.Grpc;

public static class TracingResourceBuilder
{
    public static ResourceBuilder RecommendationServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(RecommodationService.ServiceName).AddTelemetrySdk();
    }
}