using EmailService;
using EmailService.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// #### mass transit ####
builder.Services.Configure<MessageBrokerSettings>
    (builder.Configuration.GetSection(MessageBrokerSettings.SectionName));
builder.Services.AddSingleton(provider => 
    provider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
builder.Services.AddMassTransit(busConfigurator =>
{
    var assembly = typeof(IMarker).Assembly;
    busConfigurator.AddConsumers(assembly);
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
app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();