using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Common.Logger;
using JetSetGo.UsersManagement.Grpc.Common.Utility;
using JetSetGo.UsersManagement.Grpc.Keycloak;
using JetSetGo.UsersManagement.Grpc.Services;

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
        services.AddSingleton < IReservationClient,ReservationClient>();
        services.AddScoped<KeyCloakConnections>();
    }
}