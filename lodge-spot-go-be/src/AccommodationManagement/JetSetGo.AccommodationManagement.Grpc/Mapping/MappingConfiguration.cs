using AutoMapper;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using Google.Protobuf.WellKnownTypes;
using JetSetGo.AccommodationManagement.Domain.Accommodations;
using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;
using JetSetGo.AccommodationManagement.Domain.Accommodations.ValueObjects;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<Accommodation, AccommodationDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Select(photo => photo.Photo)))
            .ForMember(dest => dest.SpecialPrices, opt => opt.MapFrom(src => src.SpecalPrices));
        
        
        CreateMap<SpecialPrice, SpecialPriceDto>();
        CreateMap<DateRange, DateRangeDto>()
            .ForMember(dest => dest.From,
                opt => opt.MapFrom(src => Timestamp.FromDateTime(src.From)))
            .ForMember(dest => dest.To,
                opt => opt.MapFrom(src => Timestamp.FromDateTime(src.To)));

        CreateMap<Address, AddressDto>();
        
        CreateMap<AccommodationPhoto, PhotoDto>();
        CreateMap<SearchRequest, SearchAccommodationQuery>();
        CreateMap<SearchAccommodationResponse, AccommodationDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Select(photo => photo.Photo)))
            .ForMember(dest => dest.SpecialPrices, opt => opt.MapFrom(src => src.SpecalPrices));;
        CreateMap<Accommodation, GetAccommodationResponse>();
        CreateMap<Grade, GradeDto>();
    }
}