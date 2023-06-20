using LodgeSpotGo.ApiGateway;
using LodgeSpotGo.ApiGateway.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        b =>
        {
            b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();
//app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseOcelot().Wait();
app.UseMiddleware<CorsMiddleware>();
app.Run();