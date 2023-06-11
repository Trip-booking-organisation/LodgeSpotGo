using JetSetGo.ReservationManagement.Application;
using JetSetGo.ReservationManagement.Grpc;
using JetSetGo.ReservationManagement.Grpc.Services;
using JetSetGo.ReservationManagement.Infrastructure;
using JetSetGo.ReservationManagement.Infrastructure.MessageBroker.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc().AddJsonTranscoding();
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
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ReservationService>();
app.MapGrpcService<SearchReservationService>();
app.MapGrpcService<UserReservationService>();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.UseCors("AllowAnyOrigin");
app.Run();