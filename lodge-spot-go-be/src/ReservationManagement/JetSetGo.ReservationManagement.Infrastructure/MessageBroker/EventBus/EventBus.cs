using JetSetGo.ReservationManagement.Application.MessageBroker;
using MassTransit;

namespace JetSetGo.ReservationManagement.Infrastructure.MessageBroker.EventBus;

public class EventBus: IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }
}