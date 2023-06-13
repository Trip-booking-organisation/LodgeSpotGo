namespace JetSetGo.AccommodationManagement.Grpc.Clients;

public interface IReservationClient
{
    GetReservationsResponse GetReservationsByGuestAndHostId(Guid guestId, Guid hostId);
}