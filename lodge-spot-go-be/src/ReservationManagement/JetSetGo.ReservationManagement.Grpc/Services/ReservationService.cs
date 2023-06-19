using AutoMapper;
using Grpc.Core;
using JetSetGo.ReservationManagement.Application.CancelReservation;
using JetSetGo.ReservationManagement.Application.Clients;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.Dto.Response;
using JetSetGo.ReservationManagement.Application.Exceptions;
using JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;
using JetSetGo.ReservationManagement.Application.MessageBroker;
using JetSetGo.ReservationManagement.Application.UpdateReservationStatus;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using JetSetGo.ReservationManagement.Grpc.Clients;
using JetSetGo.ReservationManagement.Grpc.Handlers;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;
using JetSetGo.ReservationManagement.Grpc.Saga;
using LodgeSpotGo.Shared.Events.Notification;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;
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
    private readonly CreateReservationHandler _createReservationHandler;
    public const string ServiceName = "ReservationService";
    public static readonly ActivitySource ActivitySource = new("Reservation activity");


    private readonly ReservationSagaOrchestrator _sagaOrchestrator;

    public ReservationService(
        IReservationRepository reservationRepository, 
        ILogger<ReservationService> logger, 
        IMapper mapper, 
        ISender sender, 
        IMapToGrpcResponse mapToGrpcResponse, 
        IGetAccommodationClient accommodationClient, 
        IEventBus bus,
        CreateReservationHandler createReservationHandler,
        ReservationSagaOrchestrator sagaOrchestrator)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _sender = sender;
        _mapToGrpcResponse = mapToGrpcResponse;
        _accommodationClient = accommodationClient;
        _bus = bus;
        _createReservationHandler = createReservationHandler;
        _sagaOrchestrator = sagaOrchestrator;
    }
    /*[Authorize(Roles = "host,guest")]*/
    public override async Task<GetReservationListResponse> GetReservationList(GetReservationListRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var list = new GetReservationListResponse();
        var reservations = await _reservationRepository.GetAllAsync();
        _logger.LogInformation(@"List {}", reservations.ToString());
        var responseList = reservations.Select(res => _mapper.Map<ReadReservationDto>(res)).ToList();
        responseList.ForEach( dto => list.Reservations.Add(dto));
        activity?.Stop();
        return list;
    }
    /*[Authorize(Roles = "guest")]*/
    public override async Task<CreateReservationResponse> CreateReservation(
        CreateReservationRequest request, 
        ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var result = await _sagaOrchestrator.CreateSaga(request);
        activity?.Stop();
        return new CreateReservationResponse
        {
            CreatedId = $"/api/v1/reservations/{result.ReservationId}"
        };
    }
    /*[Authorize(Roles = "guest")]*/
    public override async Task<CancelReservationResponse> CancelReservation(CancelReservationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var cancelRequest =  _mapper.Map<CancelReservationCommand>(request);
        var result = await _sender.Send(cancelRequest);
        _logger.LogInformation(@"------------------Cancel reservation status : {}",result.ToString());
        await SendCanceledNotification(new Guid(request.Id));
        activity?.Stop();
        return _mapper.Map<CancelReservationResponse>(result);
    }
    /*[Authorize(Roles = "guest")]*/
    public override async Task<UpdateReservationStatusResponse> UpdateReservation(UpdateReservationStatusRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var command = _mapper.Map<UpdateReservationStatusCommand>(request.Reservation);
        var result = await _sender.Send(command);
        activity?.Stop();
        return _mapper.Map<UpdateReservationStatusResponse>(result);
    }
    /*[Authorize(Roles = "guest")]*/
    public override async Task<GetReservationsByGuestIdResponse> GetReservationsByGuestId(GetReservationsByGuestIdRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var query = _mapper.Map<GetReservationsByGuestIdQuery>(request);
        var response = await _sender.Send(query);
        var result = _mapToGrpcResponse.MapGetByGuestIdToGrpcResponse(response);
        activity?.Stop();
        return await result;
    }

    public override async Task<GetReservationByAccommodationResponse> GetReservationsByAccommodationId(GetReservationByAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var reservations = await _reservationRepository.GetReservationsByAccommodation(Guid.Parse(request.AccommodationId));
        var result = _mapToGrpcResponse.MapGetByAccommodationToGrpcResponse(reservations);
        activity?.Stop();
        return await result;
    }

    public override async Task<GetDeletedReservationsByGuestResponse> GetDeletedReservationsByGuestId(GetDeletedReservationsByGuestRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var reservations = await _reservationRepository.GetDeletedByGuest(Guid.Parse(request.GuestId));
        activity?.Stop();
        return  _mapToGrpcResponse.MapDeletedCountToGrpcResponse(reservations);
        
    }

    public override async Task<DeleteReservationResponse> DeleteReservation(DeleteReservationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        _logger.LogInformation("-------- ");
        var id = Guid.Parse(request.Id);
        await _reservationRepository.DeleteReservation(id);
        await SendCanceledNotification(id);
        activity?.Stop();
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