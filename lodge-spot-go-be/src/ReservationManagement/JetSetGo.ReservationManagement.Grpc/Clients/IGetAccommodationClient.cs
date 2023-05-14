namespace JetSetGo.ReservationManagement.Grpc.Clients;

public interface IGetAccommodationClient
{
    GetAccommodationResponse GetAccommodation(GetAccommodationRequest request);
}