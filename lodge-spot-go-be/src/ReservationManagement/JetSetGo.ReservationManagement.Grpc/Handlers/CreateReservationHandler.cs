using JetSetGo.ReservationManagement.Application.Clients;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.Dto.Response;
using JetSetGo.ReservationManagement.Application.Exceptions;
using JetSetGo.ReservationManagement.Application.MessageBroker;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using JetSetGo.ReservationManagement.Grpc.Clients;

namespace JetSetGo.ReservationManagement.Grpc.Handlers;

public class CreateReservationHandler
{
    private readonly ILogger<CreateReservationHandler> _logger;
    private readonly IGetAccommodationClient _accommodationClient;
    private readonly IEventBus _bus;
    private readonly IGetUserClient _userClient;
    private readonly IReservationRepository _reservationRepository;

    public CreateReservationHandler(
        ILogger<CreateReservationHandler> logger,
        IGetAccommodationClient accommodationClient, 
        IEventBus bus, 
        IGetUserClient userClient, 
        IReservationRepository reservationRepository)
    {
        _logger = logger;
        _accommodationClient = accommodationClient;
        _bus = bus;
        _userClient = userClient;
        _reservationRepository = reservationRepository;
    }

    public async Task<CreateReservationResponse> HandleCreateReservation(CreateReservationRequest request)
    {
        _logger.LogInformation(
            @"Request {request.Reservation}", request.Reservation);
        UserResponse user;
        try
        {
            user = _userClient.GetUserInfo(new Guid(request.Reservation.GuestId));
        }
        catch (System.Exception exception)
        {
            throw new NotFound(exception.Message); 
        }
        var accommodationRequest = new GetAccommodationRequest
        {
            Id = request.Reservation.AccommodationId
        };
        var accommodationResponse = _accommodationClient.GetAccommodation(accommodationRequest);
        if (accommodationResponse is null)
        {
            throw new NotFound("Accommodation not found");
        }
        var reservation = CreateReservation(request, user);
        _logger.LogInformation(
            @"Reservation Id {}" , reservation.Id);
        var isLap = await CheckOverlapping(reservation);
        if (isLap)
        {
            throw new BadRequest("Overlapping dates!");
        }
        if (accommodationResponse.Accommodation.MinGuests > request.Reservation.NumberOfGuests ||
            accommodationResponse.Accommodation.MaxGuests < request.Reservation.NumberOfGuests)
        {
            throw new BadRequest("You specified wrong guest number");
        }
        if (accommodationResponse.Accommodation.AutomaticConfirmation)
            reservation.ReservationStatus = ReservationStatus.Confirmed;
        return new CreateReservationResponse
        {
            Reservation = reservation,
            AccommodationResponse = accommodationResponse
        };
    }

    private static Reservation CreateReservation(CreateReservationRequest request, UserResponse user)
    {
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            AccommodationId = Guid.Parse(request.Reservation.AccommodationId),
            DateRange = new DateRange
            {
                From = request.Reservation.DateRange.From.ToDateTime(),
                To = request.Reservation.DateRange.To.ToDateTime()
            },
            ReservationStatus = MapStringToEnum(request.Reservation.Status),
            Deleted = false,
            NumberOfGuests = request.Reservation.NumberOfGuests,
            GuestId = Guid.Parse(request.Reservation.GuestId),
            GuestEmail = user.Email
        };
        return reservation;
    }

    private static ReservationStatus MapStringToEnum(string reservationStatus)
    {
        return reservationStatus switch
        {
            "Confirmed" => ReservationStatus.Confirmed,
            "Refused" => ReservationStatus.Refused,
            _ => ReservationStatus.Waiting
        };
    }
    private async Task<bool> CheckOverlapping(Reservation reservation)
    {
        var list = await _reservationRepository.GetAllAsync();
        var laps = list.Select(reservation1 => reservation1.IsOverlapping(reservation.DateRange)
                                               && reservation1.ReservationStatus == ReservationStatus.Confirmed
                                               && reservation1.AccommodationId == reservation.AccommodationId);
        return laps.Any(lap => lap);
    }
}