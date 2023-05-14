using JetSetGo.ReservationManagement.Application;
using JetSetGo.ReservationManagement.Grpc;
using JetSetGo.ReservationManagement.Grpc.Services;
using JetSetGo.ReservationManagement.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddGrpc().AddJsonTranscoding();
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

}

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
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.UseCors("AllowAnyOrigin");
app.Run();