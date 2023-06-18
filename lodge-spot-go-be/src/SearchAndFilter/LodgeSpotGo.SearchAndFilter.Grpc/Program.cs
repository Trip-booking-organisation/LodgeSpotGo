using LodgeSpotGo.SearchAndFilter.Grpc;
using LodgeSpotGo.SearchAndFilter.Grpc.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(services =>
    { services
            .AddSource(SearchAndFilterService.ServiceName)
            .SetResourceBuilder(TracingResourceBuilder.SearchAndFilterServiceResource());
        services
            .AddAspNetCoreInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddJaegerExporter()
            .SetSampler(new AlwaysOnSampler());
    });
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddPresentation();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            b =>
                b.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
        );
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
//builder.Services.AddKeycloakAuthentication(builder.Configuration, KeycloakAuthenticationOptions.Section);
builder.Services.AddAuthorization();
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
app.UseRouting();
app.UseCors("AllowOrigin");
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<FilterService>();
app.MapGrpcService<SearchAndFilterService>();
app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SearchAndFilterMicroservice v1");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();