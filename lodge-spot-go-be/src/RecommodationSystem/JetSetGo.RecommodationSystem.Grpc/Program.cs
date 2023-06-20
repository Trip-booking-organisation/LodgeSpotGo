using JetSetGo.AccommodationManagement.Grpc.Mapping;
using JetSetGo.RecommodationSystem.Grpc;
using JetSetGo.RecommodationSystem.Grpc.Services;
using LodgeSpotGo.RecommodationSystem.Core.Services;
using LodgeSpotGo.RecommodationSystem.Infrastructure;
using LodgeSpotGo.RecommodationSystem.Infrastructure.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(services =>
    { services
            .AddSource(RecommodationService.ServiceName)
            .SetResourceBuilder(TracingResourceBuilder.RecommendationServiceResource());
        services
            .AddAspNetCoreInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddJaegerExporter()
            .SetSampler(new AlwaysOnSampler());
    });
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(MappingConfiguration));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddGrpcSwagger().AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Recommendation Management Microservice",
        Version= "v1",
        Description = "Recommendation Management Microservice"
    });
});
// #### mass transit ####
builder.Services.Configure<MessageBrokerSettings>
    (builder.Configuration.GetSection(MessageBrokerSettings.SectionName));
builder.Services.AddSingleton(provider => 
    provider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
builder.Services.AddMassTransit(busConfigurator =>
{
    var assembly = typeof(IAssemblyMarker).Assembly;
    busConfigurator.AddConsumers(assembly);
    busConfigurator.AddSagaStateMachines(assembly);
    busConfigurator.AddSagas(assembly);
    busConfigurator.AddActivities(assembly);
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        var messageBrokerSettings = context.GetRequiredService<MessageBrokerSettings>();
        configurator.Host(new Uri(messageBrokerSettings.Host), hostConfigurator =>
        {
            hostConfigurator.Username(messageBrokerSettings.Username);   
            hostConfigurator.Password(messageBrokerSettings.Password);   
        });
        configurator.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
    });
});
builder.Services.AddScoped<RecommendationService>();
var app = builder.Build();
{
    app.UseSwagger().UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReservationManagementMicroservice v1");
    });
}
app.MapGrpcService<RecommodationService>();
app.MapGrpcService<UserService>();
app.MapGrpcService<AccommodationService>();
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();