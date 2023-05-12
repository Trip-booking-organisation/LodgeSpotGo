using JetSetGo.AccommodationManagement.Application;
using JetSetGo.AccommodationManagement.Grpc;
using JetSetGo.AccommodationManagement.Grpc.Services;
using JetSetGo.AccommodationManagement.Infrastructure;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddGrpc().AddJsonTranscoding();
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddAPresentation();
    var authenticationOptions = new KeycloakAuthenticationOptions
    {
        AuthServerUrl = "http://localhost:8080/",
        Realm = "jet-set-go",
        Resource = "jet-set-go",
    };
    //builder.Services.AddKeycloakAuthentication(authenticationOptions);
    builder.Services.AddGrpcSwagger().AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",new OpenApiInfo
        {
            Title = "Accommodation Management Microservice",
            Version= "v1",
            Description = "Accommodation Management Microservice"
        });
        c.CustomSchemaIds(type => type.ToString());
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "KEYCLOAK",
            Type = SecuritySchemeType.OAuth2,
            In = ParameterLocation.Header,
            BearerFormat = "JWT",
            Scheme = "bearer",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(builder.Configuration["Keycloak:AuthorizationUrl"]!),
                    TokenUrl = new Uri(builder.Configuration["Keycloak:TokenUrl"]!),
                    Scopes = new Dictionary<string, string> { }
                }
            },
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement{{securityScheme, new string[] { }}});
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