using JetSetGo.UsersManagement.Grpc.Common.Logger;
using JetSetGo.UsersManagement.Grpc.Keycloak;

namespace JetSetGo.UsersManagement.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services
        ,ConfigurationManager builder)
    {
        services.Configure<KeycloakAdmin>(builder.GetSection(KeycloakAdmin.SectionName));
        services.AddScoped<MessageLogger>();
        return services;
    }
}