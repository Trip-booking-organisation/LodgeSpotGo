using JetSetGo.AccommodationManagement.Application;
using JetSetGo.AccommodationManagement.Grpc;
using JetSetGo.AccommodationManagement.Grpc.Services;
using JetSetGo.AccommodationManagement.Infrastructure;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddGrpc().AddJsonTranscoding();
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddAPresentation();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    
    }).AddJwtBearer(o =>
    {
        o.Authority = builder.Configuration["Jwt:Authority"];
        o.Audience = builder.Configuration["Jwt:Audience"];
        o.RequireHttpsMetadata = false;
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
    builder.Services.AddKeycloakAuthorization(builder.Configuration, KeycloakAuthenticationOptions.Section);
    builder.Services
        .AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                b =>
                    b.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding"));
        });
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
                    AuthorizationUrl = new Uri(builder.Configuration["Jwt:AuthorizationUrl"]!),
                    TokenUrl = new Uri(builder.Configuration["Jwt:TokenUrl"]!),
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
    app.UseRouting();
    app.UseSwagger().UseSwaggerUI(c =>
    {
        c.OAuthClientId(builder.Configuration["Jwt:ClientId"]);
        c.OAuthClientSecret(builder.Configuration["Jwt:ClientSecret"]);
        c.OAuthRealm(builder.Configuration["Jwt:Realm"]);
        c.OAuthAppName("KEYCLOAK");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccommodationManagementMicroservice v1");
    });
    app.MapGrpcService<GreeterService>().RequireAuthorization();
    app.MapGrpcService<AccommodationService>();
    app.MapGrpcService<SearchAccommodationService>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapGet("/",
        () =>
            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    app.Run();
}