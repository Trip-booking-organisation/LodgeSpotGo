namespace LodgeSpotGo.RecommodationSystem.Core.MessageBroker;

public interface IEventBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
        where T : class;
}