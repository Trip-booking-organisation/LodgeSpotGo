using AutoMapper;
using Grpc.Core;
using JetSetGo.ReservationManagement.Application.CancelReservation;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.Exceptions;
using JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;
using JetSetGo.ReservationManagement.Application.MessageBroker;
using JetSetGo.ReservationManagement.Application.UpdateReservationStatus;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using JetSetGo.ReservationManagement.Grpc.Clients;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;
using LodgeSpotGo.Shared.Events.Reservation;
using MediatR;

namespace JetSetGo.ReservationManagement.Grpc.Services;

public class ReservationService : ReservationApp.ReservationAppBase
{
    private readonly ILogger<ReservationService> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapToGrpcResponse _mapToGrpcResponse;
    private readonly IMapper _mapper;
    private readonly ISender _sender;
    private readonly IGetAccommodationClient _accommodationClient;
    private readonly IEventBus _bus;

    public ReservationService(
        IReservationRepository reservationRepository, 
        ILogger<ReservationService> logger, 
        IMapper mapper, 
        ISender sender, 
        IMapToGrpcResponse mapToGrpcResponse, 
        IGetAccommodationClient accommodationClient, 
        IEventBus bus)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _sender = sender;
        _mapToGrpcResponse = mapToGrpcResponse;
        _accommodationClient = accommodationClient;
        _bus = bus;
    }
    /*[Authorize(Roles = "host,guest")]*/
    public override async Task<GetReservationListResponse> GetReservationList(GetReservationListRequest request, ServerCallContext context)
    {
        var list = new GetReservationListResponse();
        var reservations = await _reservationRepository.GetAllAsync();
        _logger.LogInformation(@"List {}", reservations.ToString());
        var responseList = reservations.Select(res => _mapper.Map<ReadReservationDto>(res)).ToList();
        responseList.ForEach( dto => list.Reservations.Add(dto));
        return list;
    }
    /*[Authorize(Roles = "guest")]*/
    public override async Task<CreateReservationResponse> CreateReservation(CreateReservationRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"Request {request.Reservation}", request.Reservation);
        var reservation = new Reservation
        {
            Id = new Guid(),
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
            GuestEmail = request.Reservation.GuestEmail
        };
        var accommodationRequest = new GetAccommodationRequest
        {
            Id = request.Reservation.AccommodationId
        };
        var accommodationResponse = _accommodationClient.GetAccommodation(accommodationRequest);
        if (accommodationResponse is null)
        {
            throw new NotFound("Accommodation not found");
        }
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
        await _reservationRepository.CreateAsync(reservation);
        var @event = new CreatedReservationEvent
        {
            GuestId = reservation.GuestId,
            AccommodationName = accommodationResponse.Accommodation.Name,
            AccommodationId = accommodationResponse.Accommodation.Id,
            HostId = new Guid(accommodationResponse.Accommodation.HostId),
            From = reservation.DateRange.From,
            To = reservation.DateRange.To,
            GuestEmail = reservation.GuestEmail,
        };
        await _bus.PublishAsync(@event);
        return new CreateReservationResponse
        {
            CreatedId = reservation.Id.ToString()
        };
    }

    private async Task<bool> CheckOverlapping(Reservation reservation)
    {
        var list = await _reservationRepository.GetAllAsync();
        var laps = list.Select(reservation1 => reservation1.IsOverlapping(reservation.DateRange)
                                               && reservation1.GuestId == reservation.GuestId
                                               && reservation1.AccommodationId == reservation.AccommodationId);
        return laps.Any(lap => lap);
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
    /*[Authorize(Roles = "guest")]*/
    public override async Task<CancelReservationResponse> CancelReservation(CancelReservationRequest request, ServerCallContext context)
    {
        var cancelRequest =  _mapper.Map<CancelReservationCommand>(request);
        var result = await _sender.Send(cancelRequest);
        _logger.LogInformation(@"------------------Cancel reservation status : {}",result.ToString());
        await SendCanceledNotification(new Guid(request.Id));
        return _mapper.Map<CancelReservationResponse>(result);
    }
    /*[Authorize(Roles = "guest")]*/
    public override async Task<UpdateReservationStatusResponse> UpdateReservation(UpdateReservationStatusRequest request, ServerCallContext context)
    {
        var command = _mapper.Map<UpdateReservationStatusCommand>(request.Reservation);
        var result = await _sender.Send(command);
        return _mapper.Map<UpdateReservationStatusResponse>(result);
    }
    /*[Authorize(Roles = "guest")]*/
    public override async Task<GetReservationsByGuestIdResponse> GetReservationsByGuestId(GetReservationsByGuestIdRequest request, ServerCallContext context)
    {
        var query = _mapper.Map<GetReservationsByGuestIdQuery>(request);
        var response = await _sender.Send(query);
        var result = _mapToGrpcResponse.MapGetByGuestIdToGrpcResponse(response);
        return await result;
    }

    public override async Task<GetReservationByAccommodationResponse> GetReservationsByAccommodationId(GetReservationByAccommodationRequest request, ServerCallContext context)
    {
        var reservations = await _reservationRepository.GetReservationsByAccommodation(Guid.Parse(request.AccommodationId));
        var result = _mapToGrpcResponse.MapGetByAccommodationToGrpcResponse(reservations);
        return await result;
    }

    public override async Task<GetDeletedReservationsByGuestResponse> GetDeletedReservationsByGuestId(GetDeletedReservationsByGuestRequest request, ServerCallContext context)
    {
        var reservations = await _reservationRepository.GetDeletedByGuest(Guid.Parse(request.GuestId));
        return  _mapToGrpcResponse.MapDeletedCountToGrpcResponse(reservations);
        
    }

    public override async Task<DeleteReservationResponse> DeleteReservation(DeleteReservationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("-------- ");
        var id = Guid.Parse(request.Id);
        await _reservationRepository.DeleteReservation(id);
        await SendCanceledNotification(id);
        return new DeleteReservationResponse
        {
            Success = true
        };
    }

    private async Task SendCanceledNotification(Guid id)
    {
        var reservation = await _reservationRepository.GetById(id);
        var accommodationRequest = new GetAccommodationRequest
        {
            Id = reservation.AccommodationId.ToString()
        };
        var accommodationResponse = _accommodationClient.GetAccommodation(accommodationRequest);
        var @event = new CanceledReservationEvent
        {
            GuestId = reservation.GuestId,
            GuestEmail = reservation.GuestEmail,
            AccommodationName = accommodationResponse.Accommodation.Name,
            AccommodationId = accommodationResponse.Accommodation.Id,
            HostId = new Guid(accommodationResponse.Accommodation.HostId),
            CancelTime = DateTime.Now
        };
        await _bus.PublishAsync(@event);
    }
}