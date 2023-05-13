using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using ZstdSharp;

namespace JetSetGo.ReservationManagement.Grpc.Mapping;

public class MappingConfiguration:Profile
{
    public MappingConfiguration()
    {
        CreateMap<Reservation, CreateReservationDto>().ForMember(dest => dest.DateRange,
            opt =>
                opt.MapFrom(src => src.DateRange));

        CreateMap<DateRange, DateRangeDto>()
            .ForMember(dest => dest.From,
                opt => opt.MapFrom(src => src.From.ToTimestamp()))
            .ForMember(dest => dest.To,
                opt => opt.MapFrom(src => src.To.ToTimestamp()));;
        CreateMap<Reservation, ReadReservationDto>().ForMember(dest => dest.DateRange,
            opt =>
                opt.MapFrom(src => src.DateRange));
        CreateMap<ReservationSearchRequest, SearchReservationsQuery>();
        CreateMap<SearchReservationResponse, ReadReservationDto>()
            .ForMember(dest => dest.DateRange,
            opt => opt.MapFrom(src => src.DateRange));
    }
    
}