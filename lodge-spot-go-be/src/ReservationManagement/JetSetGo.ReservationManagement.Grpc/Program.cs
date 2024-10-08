using Grpc.Core;
using Grpc.Net.Client;
using JetSetGo.ReservationManagement.Application;
using JetSetGo.ReservationManagement.Grpc;
using JetSetGo.ReservationManagement.Grpc.Interceptors;
using JetSetGo.ReservationManagement.Grpc.Middleware;
using JetSetGo.ReservationManagement.Grpc.Saga;
using JetSetGo.ReservationManagement.Grpc.Saga.States;
using JetSetGo.ReservationManagement.Grpc.Services;
using JetSetGo.ReservationManagement.Infrastructure;
using JetSetGo.ReservationManagement.Infrastructure.MessageBroker.Settings;
using LodgeSpotGo.Shared.Events.Notification;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(services =>
    { services
            .AddSource(ReservationService.ServiceName)
            .SetResourceBuilder(TracingResourceBuilder.ReservationServiceResource());
        services
            .AddAspNetCoreInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddJaegerExporter()
            .SetSampler(new AlwaysOnSampler());
    });
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
    options.Interceptors.Add<LoggingInterceptor>();
    options.EnableDetailedErrors = true;
}).AddJsonTranscoding();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddPresentation()
    .AddApplication();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        b =>
        {
            b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Services.AddGrpcSwagger().AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Reservation Management Microservice",
        Version= "v1",
        Description = "Reservation Management Microservice"
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
    busConfigurator.AddSagaStateMachine<ReservationStateMachine,
        ReservationState>().InMemoryRepository();
    busConfigurator.AddRequestClient<CreateNotificationCommand>(new Uri("exchange:notification-status"));
    busConfigurator.AddRequestClient<NotificationCreatedEvent>(new Uri("exchange:email-status"));
    busConfigurator.AddSagas(assembly);
    busConfigurator.AddActivities(assembly);
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        var messageBrokerSettings = context.GetRequiredService<MessageBrokerSettings>();
        configurator.Host(messageBrokerSettings.Host, hostConfigurator =>
        {
            hostConfigurator.Username(messageBrokerSettings.Username);   
            hostConfigurator.Password(messageBrokerSettings.Password);   
        });
        configurator.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    
}).AddJwtBearer(o =>
{
    o.Authority = builder.Configuration["Jwt:Authority"];
    o.Audience = builder.Configuration["Jwt:Audience"];
    o.TokenValidationParameters = new TokenValidationParameters{
        ValidateAudience = false,
    };
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters.ValidIssuers = new[]
    {
        builder.Configuration["Jwt:Authority"]
    };
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();
    
            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
    
            return c.Response.WriteAsync(builder.Environment.IsDevelopment() 
                ? c.Exception.ToString() 
                : "An error occured processing your authentication.");
        }
    };
});
builder.Services.AddAuthorization();
var app = builder.Build();
{
    app.UseSwagger().UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReservationManagementMicroservice v1");
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<ReservationService>();
app.MapGrpcService<SearchReservationService>();
app.MapGrpcService<UserReservationService>();
app.MapGrpcService<GetReservationByAccomAndGuestService>();
app.MapGrpcService<ReservationAccommodationHost>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.UseCors("AllowAnyOrigin");
app.Run();