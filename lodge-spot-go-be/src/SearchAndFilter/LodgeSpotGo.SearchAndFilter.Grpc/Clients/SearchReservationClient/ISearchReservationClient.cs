namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchReservationClient;

public interface ISearchReservationClient
{
    GetReservationListResponse SearchReservations(ReservationSearchRequest request);
}