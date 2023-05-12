using AutoMapper;
using JetSetGo.AccommodationManagement.Domain.Accommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodation.ValueObjects;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Accommodation, AccommodationDto>().ForMember(dest => dest.Address,
            opt => 
                opt.MapFrom(src => src.Address));
        CreateMap<Address, AddressDto>();
        CreateMap<Accommodation, AccommodationDto>().ForMember(dest => dest.Photos,
            opt => 
                opt.MapFrom(src => src.Photos));
        CreateMap<AccommodationPhoto, PhotoDto>();
        CreateMap<Accommodation, AccommodationResponse>();
    }
}