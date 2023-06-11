using JetSetGo.ReservationManagement.Application.MessageBroker;
using MassTransit;

namespace JetSetGo.ReservationManagement.Infrastructure.MessageBroker.EventBus;

public class EventBus: IEventBus
{
    private readonly IBus _bus;

    public EventBus(IBus bus)
    {
        _bus = bus;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        return _bus.Publish(message, cancellationToken);
    }
}