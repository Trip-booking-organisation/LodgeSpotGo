using JetSetGo.ReservationManagement.Application.Clients;
using JetSetGo.ReservationManagement.Grpc.Clients;
using JetSetGo.ReservationManagement.Grpc.Clients.GetUserClient;
using JetSetGo.ReservationManagement.Grpc.Handlers;
using JetSetGo.ReservationManagement.Grpc.Mapping;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;
using JetSetGo.ReservationManagement.Grpc.Saga;

namespace JetSetGo.ReservationManagement.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfiguration));
        services.AddScoped<IMapToGrpcResponse, MapToGrpcResponse>();
        services.AddScoped<IGetAccommodationClient, GetAccommodationClient>();
        services.AddScoped<IClientAccommodationMediator,GetAccommodationClientMediator>();
        services.AddScoped<IGetUserClient,GetUserInfosClient>();
        services.AddScoped<CreateReservationHandler>();
        services.AddScoped<ReservationSagaOrchestrator>();
        return services;
    }
}