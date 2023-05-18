using AutoMapper;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodation;

namespace JetSetGo.AccommodationManagement.Application.Mapping;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Accommodation, SearchAccommodationResponse>()
            .ForMember(dest => dest.SpecalPrices, opt => opt.MapFrom(src => src.SpecalPrices));
    }
}