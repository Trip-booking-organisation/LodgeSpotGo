using JetSetGo.ReservationManagement.Grpc.Services;
using OpenTelemetry.Resources;

namespace JetSetGo.ReservationManagement.Grpc;

public static class TracingResourceBuilder
{
    public static ResourceBuilder ReservationServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(ReservationService.ServiceName);
    }
  
}