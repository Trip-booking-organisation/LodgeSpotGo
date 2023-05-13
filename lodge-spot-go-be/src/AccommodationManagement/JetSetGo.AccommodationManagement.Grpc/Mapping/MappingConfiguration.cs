using AutoMapper;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using Google.Protobuf.WellKnownTypes;
using JetSetGo.AccommodationManagement.Domain.Accommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodation.ValueObjects;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Accommodation, AccommodationDto>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
            .ForMember(dest => dest.SpecialPrices, opt => opt.MapFrom(src => src.SpecalPrices));
        
        
        CreateMap<SpecalPrice, SpecialPriceDto>();
        CreateMap<DateRange, DateRangeDto>()
            .ForMember(dest => dest.From,
                opt => opt.MapFrom(src => Timestamp.FromDateTime(src.From)))
            .ForMember(dest => dest.To,
                opt => opt.MapFrom(src => Timestamp.FromDateTime(src.To)));

        CreateMap<Address, AddressDto>();
        
        CreateMap<AccommodationPhoto, PhotoDto>();
        CreateMap<SearchRequest, SearchAccommodationQuery>();
        CreateMap<SearchAccommodationResponse, AccommodationDto>();
    }
}