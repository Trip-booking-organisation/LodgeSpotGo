using AutoMapper;
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

        CreateMap<DateRange, DateRangeDto>();
        CreateMap<Reservation, ReadReservationDto>().ForMember(dest => dest.DateRange,
            opt =>
                opt.MapFrom(src => src.DateRange));
    }
    
}