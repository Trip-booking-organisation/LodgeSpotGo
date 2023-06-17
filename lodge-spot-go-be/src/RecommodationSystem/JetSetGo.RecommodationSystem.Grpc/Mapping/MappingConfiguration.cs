using AutoMapper;
using JetSetGo.RecommodationSystem.Grpc;
using LodgeSpotGo.RecommodationSystem.Core.Model;


namespace JetSetGo.AccommodationManagement.Grpc.Mapping;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Guest, UserRecDto>();
        CreateMap<UserRecDto, Guest>();
    }
}