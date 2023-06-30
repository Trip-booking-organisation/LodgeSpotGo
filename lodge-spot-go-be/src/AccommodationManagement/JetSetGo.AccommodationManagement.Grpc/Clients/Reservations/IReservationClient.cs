namespace JetSetGo.AccommodationManagement.Grpc.Clients.Reservations;

public interface IReservationClient
{
    GetReservationsResponse GetReservationsByGuestAndHostId(Guid guestId, Guid hostId);
}