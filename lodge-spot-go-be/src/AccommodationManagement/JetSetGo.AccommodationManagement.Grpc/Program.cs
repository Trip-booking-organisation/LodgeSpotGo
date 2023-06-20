using System.Reflection;
using JetSetGo.AccommodationManagement.Application;
using JetSetGo.AccommodationManagement.Grpc;
using JetSetGo.AccommodationManagement.Grpc.Services;
using JetSetGo.AccommodationManagement.Grpc.Services.Grades;
using JetSetGo.AccommodationManagement.Infrastructure;
using JetSetGo.AccommodationManagement.Infrastructure.MessageBroker.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOpenTelemetry()
        .WithTracing(services =>
        {
            services
                .AddSource(AccommodationService.ServiceName)
                .SetResourceBuilder(TracingResourceBuilder.AccommodationServiceResource())
                .AddAspNetCoreInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddJaegerExporter()
                .SetSampler(new AlwaysOnSampler());
        });
    builder.Services.AddGrpc().AddJsonTranscoding();
    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPresentation(builder.Configuration);
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
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(80, o => o.Protocols =
            HttpProtocols.Http2);
        options.ListenLocalhost(443, o => o.Protocols =
            HttpProtocols.Http1);
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
    // builder.Services.AddKeycloakAuthorization(builder.Configuration, KeycloakAuthenticationOptions.Section);
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
    
// #### mass transit ####
    builder.Services.Configure<MessageBrokerSettings>
        (builder.Configuration.GetSection(MessageBrokerSettings.SectionName));
    builder.Services.AddSingleton(provider => 
        provider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
    builder.Services.AddMassTransit(busConfigurator =>
    {
        var assembly = typeof(IAssemblyMarker).Assembly;
        busConfigurator.AddConsumers(assembly);
        busConfigurator.AddSagaStateMachines(assembly);
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
    app.UseHttpsRedirection();
    app.UseCors("AllowOrigin");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapGrpcService<GreeterService>()/*.RequireAuthorization()*/;
    app.MapGrpcService<AccommodationService>();
    app.MapGrpcService<GetAccommodationService>();
    app.MapGrpcService<SearchAccommodationService>();
    app.MapGrpcService<GradeService>();
    app.MapGrpcService<FilterGrades>();
    app.MapGrpcService<HostAccommodationService>();
    app.MapGet("/",
        () =>
            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, " +
            "visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    app.Run();
}