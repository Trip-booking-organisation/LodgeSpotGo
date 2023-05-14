using JetSetGo.UsersManagement.Api.Common.Logger;
using JetSetGo.UsersManagement.Api.Keycloak;

namespace JetSetGo.UsersManagement.Api;

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