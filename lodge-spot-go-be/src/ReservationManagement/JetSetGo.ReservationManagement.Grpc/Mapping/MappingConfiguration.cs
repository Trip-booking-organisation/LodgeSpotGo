using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using JetSetGo.ReservationManagement.Application.CancelReservation;
using JetSetGo.ReservationManagement.Application.Clients.Responses;
using JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Application.UpdateReservationStatus;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using ZstdSharp;

namespace JetSetGo.ReservationManagement.Grpc.Mapping;

public class MappingConfiguration:Profile
{
    public MappingConfiguration()
    {
        CreateMap<Reservation, CreateReservationDto>()
            .ForMember(dest => dest.DateRange,
            opt =>
                opt.MapFrom(src => src.DateRange));

        CreateMap<DateRange, DateRangeDto>()
            .ForMember(dest => dest.From,
                opt =>
                    opt.MapFrom(src => src.From.ToTimestamp()))
            .ForMember(dest => dest.To,
                opt => 
                    opt.MapFrom(src => src.To.ToTimestamp()));;
        CreateMap<Reservation, ReadReservationDto>().ForMember(dest => dest.DateRange,
            opt =>
                opt.MapFrom(src => src.DateRange))
            .ForMember(dest => dest.Status,
                opt => 
                    opt.MapFrom(src => MapEnumToString(src.ReservationStatus)));
        CreateMap<ReservationSearchRequest, SearchReservationsQuery>();
        CreateMap<SearchReservationResponse, ReadReservationDto>()
            .ForMember(dest => dest.DateRange,
            opt => 
                opt.MapFrom(src => src.DateRange));
        CreateMap<CancelReservationRequest, CancelReservationCommand>();
        CreateMap<CancelReservationCommandResponse, CancelReservationResponse>();
        CreateMap<UpdateReservationStatus, UpdateReservationStatusCommand>();
        CreateMap<UpdateReservationStatusCommandResponse, UpdateReservationStatusResponse>();
        CreateMap<GetReservationsByGuestIdRequest, GetReservationsByGuestIdQuery>()
            .ForMember(dest => dest.GuestId, 
                opt => 
                    opt.MapFrom(src => Guid.Parse(src.GuestId)));
        CreateMap<GetReservationsByGuestIdCommandResponse, ReadReservationDto>().ForMember(dest => dest.DateRange,
            opt => 
                opt.MapFrom(src => src.DateRange))
            .ForMember(dest => dest.Status,
                opt => 
                    opt.MapFrom(src => MapEnumToString(src.ReservationStatus)))
            .ForMember(dest => dest.NumberOfGuest, opt =>
        opt.MapFrom(src => src.NumberOfGuests));
        CreateMap<DateRange, DateRangeReservation>()
            .ForMember(dest => dest.From,
                opt =>
                    opt.MapFrom(src => src.From.ToTimestamp()))
            .ForMember(dest => dest.To,
                opt => 
                    opt.MapFrom(src => src.To.ToTimestamp()));
        CreateMap<Reservation, GetReservationDto>()
            .ForMember(dest => dest.DateRange,
                opt =>
                    opt.MapFrom(src => src.DateRange));
        CreateMap<DateRange, DateRangeReservationHost>()
            .ForMember(dest => dest.From,
                opt =>
                    opt.MapFrom(src => src.From.ToTimestamp()))
            .ForMember(dest => dest.To,
                opt => 
                    opt.MapFrom(src => src.To.ToTimestamp()));;
        CreateMap<Reservation, GetReservationAccommodation>()
            .ForMember(dest => dest.DateRange,
                opt =>
                    opt.MapFrom(src => src.DateRange))
            .ForMember(dest => dest.Deleted,  
                opt =>opt.MapFrom(src => src.Deleted));
        CreateMap<AccommodationDtoResponse, GetAccommodationResponse>();
        CreateMap<GetAccommodationResponse, AccommodationDtoResponse>()
            .ForMember(destinationMember => 
                destinationMember.Name, opt => 
                opt.MapFrom(dest => dest.Accommodation.Name));
    }

    public string MapEnumToString(ReservationStatus status)
    {
        return status switch
        {
            ReservationStatus.Confirmed =>"Confirmed"  ,
            ReservationStatus.Refused =>"Refused",
            _ => "Waiting"
        };
    }
    
}