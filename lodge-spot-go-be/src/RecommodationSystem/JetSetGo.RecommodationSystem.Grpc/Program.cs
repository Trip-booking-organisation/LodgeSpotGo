using JetSetGo.RecommodationSystem.Grpc.Services;
using LodgeSpotGo.RecommodationSystem.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
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