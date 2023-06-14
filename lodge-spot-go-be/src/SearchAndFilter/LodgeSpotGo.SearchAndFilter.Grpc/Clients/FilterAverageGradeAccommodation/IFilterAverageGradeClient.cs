namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.FilterAverageGradeAccommodation;

public interface IFilterAverageGradeClient
{
    GetAverageGradeByAccommodationResponse GetAverageGradeForAccommodation(Guid id);
}