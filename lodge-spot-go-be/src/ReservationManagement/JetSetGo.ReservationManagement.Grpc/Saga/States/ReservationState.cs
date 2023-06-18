using JetSetGo.ReservationManagement.Domain.Reservation;
using MassTransit;
using MassTransit.Futures.Contracts;

namespace JetSetGo.ReservationManagement.Grpc.Saga.States;

public class ReservationState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; } = null!;
    public Reservation? Reservation { get; set; }
}