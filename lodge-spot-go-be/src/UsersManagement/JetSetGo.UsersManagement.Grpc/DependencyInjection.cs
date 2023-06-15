using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Client.Accommodations;
using JetSetGo.UsersManagement.Grpc.Common.Logger;
using JetSetGo.UsersManagement.Grpc.Common.Utility;
using JetSetGo.UsersManagement.Grpc.Keycloak;
using JetSetGo.UsersManagement.Grpc.Services;
using JetSetGo.UsersManagement.Infrastructure.Persistence.Repository;

namespace JetSetGo.UsersManagement.Grpc;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services
        , ConfigurationManager builder)
    {
        services.Configure<KeycloakAdmin>(builder.GetSection(KeycloakAdmin.SectionName));
        services.AddGrpc();
        services.AddScoped<MessageLogger>();
        services.AddHttpClient();
        services.AddScoped<MyUserGrpcService>();
        services.AddScoped<TokenService>();
        services.AddScoped<GradesGrpcService>();
        services.AddScoped<HostService>();
        services.AddScoped< IReservationClient,ReservationClient>();
        services.AddScoped< IAccommodationClient,AccommodationClient>();
        services.AddSingleton < IHostGradeRepository,HostGradeRepository>();
        services.AddScoped<KeyCloakConnections>();
    }
}