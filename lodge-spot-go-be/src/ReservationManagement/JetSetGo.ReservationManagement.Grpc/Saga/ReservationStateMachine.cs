using JetSetGo.ReservationManagement.Grpc.Saga.States;
using LodgeSpotGo.Shared.Events.Notification;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;

namespace JetSetGo.ReservationManagement.Grpc.Saga;

public class ReservationStateMachine : MassTransitStateMachine<ReservationState>
{
    private readonly ILogger<ReservationStateMachine> _logger;
    public ReservationStateMachine(ILogger<ReservationStateMachine> logger)
    {
        _logger = logger;
        InstanceState(state => state.CurrentState);
        State(() => CreatingReservation);
        ConfigureCorrelationIds();
        When(CreatedReservation).Then(
            c => _logger.LogInformation("Recieved {}",c.Message.AccommodationName));
        When(CreatedNotification).Then(
            c => _logger.LogInformation("Recieved {}", c.Message.ReservationId));
    }

    private void ConfigureCorrelationIds()
    {
        Event(() => CreatedReservation, 
            x => 
                x.CorrelateById(m => m.Message.ReservationId));
        Event(() => CreatedNotification, 
            x => 
                x.CorrelateById(m => m.Message.ReservationId));
    }
    

    public Event<CreatedReservationEvent> CreatedReservation { get; private set; } = null!;
    public Event<NotificationCreatedEvent> CreatedNotification { get; private set; } = null!;
    public State CreatingReservation { get; private set; } = null!;
    public State ReservationCreatedState { get; private set; } = null!;
    public State ReservationCreationFailedState { get; private set; } = null!;
    public State EmailSendState { get; private set; } = null!;
    public State EmailSendFailedState { get; private set; } = null!;
}