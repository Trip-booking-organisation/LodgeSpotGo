using JetSetGo.AccommodationManagement.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddGrpc().AddJsonTranscoding();
    builder.Services.AddGrpcSwagger().AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",new(){
            Title = "Accommodation Management Microservice",
            Version= "v1",
            Description = "Accommodation Management Microservice"
        });
    });
}

var app = builder.Build();
{
    app.UseSwagger().UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccommodationManagementMicroservice v1");
    });
    app.MapGrpcService<GreeterService>();
    app.MapGrpcService<AccommodationService>();
    app.MapGet("/",
        () =>
            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    
    app.Run();
}