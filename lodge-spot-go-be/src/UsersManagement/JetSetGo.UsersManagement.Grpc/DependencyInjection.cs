using JetSetGo.UsersManagement.Grpc.Common.Logger;
using JetSetGo.UsersManagement.Grpc.Keycloak;
using JetSetGo.UsersManagement.Grpc.Services;

namespace JetSetGo.UsersManagement.Grpc;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services
        , ConfigurationManager builder)
    {
        services.Configure<KeycloakAdmin>(builder.GetSection(KeycloakAdmin.SectionName));
        services.AddScoped<MessageLogger>();
        services.AddScoped<UserGrpcService>();
    }
}