using AutoMapper;
namespace LodgeSpotGo.SearchAndFilter.Grpc.Mapper;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<SearchAccommodationRequest, SearchRequest>();
        CreateMap<SearchAccommodationRequest,ReservationSearchRequest>();

    }
    
}