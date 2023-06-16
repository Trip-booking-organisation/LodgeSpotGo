using LodgeSpotGo.Notifications.Api;
using LodgeSpotGo.Notifications.Api.Endpoints;
using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Core;
using LodgeSpotGo.Notifications.Infrastructure;
using LodgeSpotGo.Notifications.Infrastructure.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
services.
    AddCore().
    AddInfrastructure(configuration);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            b => b
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:4200")
        );
    });
// #### mass transit ####
services.Configure<MessageBrokerSettings>
    (configuration.GetSection(MessageBrokerSettings.SectionName));
services.AddSingleton(provider => 
    provider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
services.AddMassTransit(busConfigurator =>
{
    var assembly = typeof(IAssemblyMarkerApi).Assembly;
    busConfigurator.AddConsumers(typeof(IAssemblyMarkerApi).Assembly);
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
        configurator.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
    });
});
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowOrigin");
//app.UseAuthorization();
app.MapNotificationEndpoints();
app.MapHub<NotificationsHub>("/notifications");


app.Run();