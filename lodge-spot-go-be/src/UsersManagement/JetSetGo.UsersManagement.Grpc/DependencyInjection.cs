using JetSetGo.UserManagement.Grpc;
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
        services.AddScoped<MessageLogger>();
        services.AddScoped<MyUserGrpcService>();
        services.AddScoped<TokenService>();
    }
}