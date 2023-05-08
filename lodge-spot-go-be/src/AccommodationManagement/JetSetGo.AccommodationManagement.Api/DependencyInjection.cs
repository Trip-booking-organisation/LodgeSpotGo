namespace JetSetGo.AccommodationManagement.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services
        , ConfigurationManager builderConfiguration)
    {
        services.AddEndpointsApiExplorer();
        services
            .AddSwagger()
            .AddCorsPolicy(builderConfiguration);
        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    private static void AddCorsPolicy(this IServiceCollection services, IConfiguration builderConfiguration)
    {
        var corsSection = builderConfiguration.GetSection("Cors");
        var policyName = corsSection.GetSection("PolicyName").Value!;
        var origins = corsSection.GetSection("Origins").Value!.Split(";");
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, 
                builder => builder
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
}