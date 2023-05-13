namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchAccommodationClient;

public interface ISearchAccommodationClient
{
    GetAccommodationListResponse SearchAccommodation(SearchRequest request);
}