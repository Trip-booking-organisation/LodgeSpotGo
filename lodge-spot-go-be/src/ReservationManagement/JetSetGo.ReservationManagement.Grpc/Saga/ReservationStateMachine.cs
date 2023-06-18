// using JetSetGo.ReservationManagement.Grpc.Saga.States;
// using LodgeSpotGo.Shared.Events.Notification;
// using LodgeSpotGo.Shared.Events.Reservation;
// using MassTransit;
//
// namespace JetSetGo.ReservationManagement.Grpc.Saga;
//
// public class ReservationStateMachine : MassTransitStateMachine<ReservationState>
// {
//     public ReservationStateMachine()
//     {
//         InstanceState(state => state.CurrentState);
//         State(() => CreatingReservation);
//         ConfigureCorrelationIds();
//         When(CreatedReservation).Then(
//             c => Console.WriteLine("Recieved {0}",c.Message.AccommodationName));
//         When(CreatedNotification).Then(
//             c => Console.WriteLine("Recieved {0}",c.Message.ReservationId));
//     }
//
//     private void ConfigureCorrelationIds()
//     {
//         Event(() => CreatedReservation, 
//             x => 
//                 x.CorrelateById(m => m.Message.ReservationId));
//         Event(() => CreatedNotification, 
//             x => 
//                 x.CorrelateById(m => m.Message.ReservationId));
//     }
//     
//
//     public Event<CreatedReservationEvent> CreatedReservation;
//     public Event<NotificationCreatedEvent> CreatedNotification;
//     public State CreatingReservation { get; private set; }
//     public State ReservationCreatedState { get; private set; }
//     public State ReservationCreationFailedState { get; private set; }
//     public State EmailSendState { get; private set; }
//     public State EmailSendFailedState { get; private set; }
// }