namespace JetSetGo.UsersManagement.Grpc.Client;

public interface IReservationClient
{
    GetReservationsResponse GetReservationsByGuestAndHostId(Guid guestId, Guid hostId);
}