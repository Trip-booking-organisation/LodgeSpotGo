using AutoMapper;
using JetSetGo.SearchAndFilter.Application.SearchAccommodation;
using LodgeSpotGo.SearchAndFilter.Grpc.Dto.Requests;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Mapper;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<SearchAccommodationRequest, SearchRequest>();
       
    }
    
}