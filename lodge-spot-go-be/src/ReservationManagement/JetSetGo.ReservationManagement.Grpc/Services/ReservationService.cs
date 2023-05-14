using AutoMapper;
using Grpc.Core;
using JetSetGo.ReservationManagement.Application.CancelReservation;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;
using JetSetGo.ReservationManagement.Application.UpdateReservationStatus;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using JetSetGo.ReservationManagement.Grpc.Clients;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;
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

    public ReservationService(IReservationRepository reservationRepository, ILogger<ReservationService> logger, IMapper mapper, ISender sender, IMapToGrpcResponse mapToGrpcResponse, IGetAccommodationClient accommodationClient)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _sender = sender;
        _mapToGrpcResponse = mapToGrpcResponse;
        _accommodationClient = accommodationClient;
    }

    public override async Task<GetReservationListResponse> GetReservationList(GetReservationListRequest request, ServerCallContext context)
    {
        var list = new GetReservationListResponse();
        var reservations = await _reservationRepository.GetAllAsync();
        _logger.LogInformation(@"List {}", reservations.ToString());
        var responseList = reservations.Select(res => _mapper.Map<ReadReservationDto>(res)).ToList();
        responseList.ForEach( dto => list.Reservations.Add(dto));
        return list;
    }

    public override Task<CreateReservationResponse> CreateReservation(CreateReservationRequest request, ServerCallContext context)
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
            GuestId = Guid.Parse(request.Reservation.GuestId)
        };
        var accommodationRequest = new GetAccommodationRequest
        {
            Id = request.Reservation.AccommodationId
        };
        var accommodationResponse = _accommodationClient.GetAccommodation(accommodationRequest);
        if (accommodationResponse.Accommodation.AutomaticConfirmation)
            reservation.ReservationStatus = ReservationStatus.Confirmed;
        _reservationRepository.CreateAsync(reservation);
        return Task.FromResult(new CreateReservationResponse
        {
            CreatedId = reservation.Id.ToString()
        });
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

    public override async Task<CancelReservationResponse> CancelReservation(CancelReservationRequest request, ServerCallContext context)
    {
        var cancelRequest =  _mapper.Map<CancelReservationCommand>(request);
        var result = await _sender.Send(cancelRequest);
        _logger.LogInformation(@"------------------Cancel reservation status : {}",result.ToString());
       return _mapper.Map<CancelReservationResponse>(result);
    }

    public override async Task<UpdateReservationStatusResponse> UpdateReservation(UpdateReservationStatusRequest request, ServerCallContext context)
    {
        var command = _mapper.Map<UpdateReservationStatusCommand>(request.Reservation);
        var result = await _sender.Send(command);
        return _mapper.Map<UpdateReservationStatusResponse>(result);
    }

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
        await _reservationRepository.DeleteReservation(Guid.Parse(request.Id));
        return new DeleteReservationResponse
        {
            Success = true
        };
    }
}