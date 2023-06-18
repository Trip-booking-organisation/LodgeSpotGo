using JetSetGo.AccommodationManagement.Grpc.Clients.Reservations;
using JetSetGo.AccommodationManagement.Grpc.Clients.Users;
using JetSetGo.AccommodationManagement.Grpc.Services;
using JetSetGo.AccommodationManagement.Grpc.Services.Grades;
using OpenTelemetry.Resources;

namespace JetSetGo.AccommodationManagement.Grpc;

public static class TracingResourceBuilder
{
    public static ResourceBuilder GradeServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(GradeService.ActivitySource.Name);
    }
    public static ResourceBuilder AccommodationServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(AccommodationService.ActivitySource.Name);
    }
    public static ResourceBuilder GetAccommodationServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(GetAccommodationService.ActivitySource.Name);
    }
    public static ResourceBuilder FilterGradesServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(FilterGrades.ActivitySource.Name);
    }
    public static ResourceBuilder HostAccommodationServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(HostAccommodationService.ActivitySource.Name);
    }
    public static ResourceBuilder SearchAccommodationServiceResource()
    {
        return ResourceBuilder.CreateDefault().AddService(SearchAccommodationService.ActivitySource.Name);
    }
    public static ResourceBuilder ReservationClientResource()
    {
        return ResourceBuilder.CreateDefault().AddService(ReservationClient.ActivitySource.Name);
    }
    public static ResourceBuilder UserClientResource()
    {
        return ResourceBuilder.CreateDefault().AddService(UserClient.ActivitySource.Name);
    }
}