using LodgeSpotGo.SearchAndFilter.Grpc;
using LodgeSpotGo.SearchAndFilter.Grpc.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddAPresentation();
builder.Services.AddGrpcSwagger().AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Search And Filter Microservice",
        Version= "v1",
        Description = "Search and Filter Microservice"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<SearchAndFilterService>();
app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SearchAndFilterMicroservice v1");
});
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();