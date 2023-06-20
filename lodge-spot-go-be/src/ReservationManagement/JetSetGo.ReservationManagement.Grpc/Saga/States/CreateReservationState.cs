namespace JetSetGo.ReservationManagement.Grpc.Saga.States;

public enum CreateReservationState
{
    Initial,
    CreatingReservation,
    ReservationCreatingFailed,
    NotificationReceived,
    NotificationReceivedFailed,
    EmailSend,
    EmailSendFail,
}