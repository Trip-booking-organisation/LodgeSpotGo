using LodgeSpotGo.Notifications.Api.Endpoints;
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
// #### mass transit ####
services.Configure<MessageBrokerSettings>
    (configuration.GetSection(MessageBrokerSettings.SectionName));
services.AddSingleton(provider => 
    provider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
services.AddMassTransit(busConfigurator =>
{
    var assembly = typeof(IAssemblyMarker).Assembly;
    busConfigurator.AddConsumers(typeof(IAssemblyMarker).Assembly);
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
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapNotificationEndpoints();
//app.UseAuthorization();

app.Run();