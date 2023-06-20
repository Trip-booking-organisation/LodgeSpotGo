using Grpc.Core;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.MessageBroker;
using JetSetGo.ReservationManagement.Grpc.Clients;
using JetSetGo.ReservationManagement.Grpc.Handlers;
using JetSetGo.ReservationManagement.Grpc.Saga.States;
using LodgeSpotGo.Shared.Events.Email;
using LodgeSpotGo.Shared.Events.Notification;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;

namespace JetSetGo.ReservationManagement.Grpc.Saga;

public class ReservationSagaOrchestrator: ISagaOrchestrator<CreateReservationRequest,ReservationSagaResponse>
{
    private readonly ILogger<ReservationSagaOrchestrator> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventBus _bus;
    private readonly CreateReservationHandler _createReservationHandler;
    private readonly IRequestClient<CreateNotificationCommand> _client;
    private readonly IRequestClient<SendEmailCommand> _emailClient;

    public ReservationSagaOrchestrator(
        ILogger<ReservationSagaOrchestrator> logger, 
        IReservationRepository reservationRepository,
        IEventBus bus, 
        CreateReservationHandler createReservationHandler, 
        IRequestClient<CreateNotificationCommand> client, 
        IRequestClient<SendEmailCommand> emailClient)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _bus = bus;
        _createReservationHandler = createReservationHandler;
        _client = client;
        _emailClient = emailClient;
    }

    public async Task<ReservationSagaResponse> CreateSaga(CreateReservationRequest request)
    {
         // step 1 initialize reservation
         
        var reservation = await _createReservationHandler.HandleCreateReservation(request);
        var createNotificationCommand = new CreateNotificationCommand
        {
            ReservationId = reservation.Reservation.Id,
            GuestId = reservation.Reservation.GuestId,
            AccommodationName = reservation.AccommodationResponse.Accommodation.Name,
            AccommodationId =  reservation.AccommodationResponse.Accommodation.Id,
            HostId = new Guid(reservation.AccommodationResponse.Accommodation.HostId),
            From = reservation.Reservation.DateRange.From,
            To = reservation.Reservation.DateRange.To,
            GuestEmail = reservation.Reservation.GuestEmail,
        };
        
        // step 2 create and get notification
        var response = await SendNotificationCommand(createNotificationCommand);
        
        // step 3 send email
        await SendEmailCommand(response);
        
        // step 4 create reservation and publish it
        await _reservationRepository.CreateAsync(reservation.Reservation);
        await PublishEvent(reservation);
        return new ReservationSagaResponse
        {
            ReservationId = reservation.Reservation.Id.ToString()
        };
    }

    private async Task<Response<NotificationCreatedEvent>> SendNotificationCommand(CreateNotificationCommand createNotificationCommand)
    {
        Response<NotificationCreatedEvent>? response;
        try
        {
            response = await _client.GetResponse<NotificationCreatedEvent>(createNotificationCommand);
            _logger.LogInformation("Received notification created {}", response.Message.Content);
        }
        catch (System.Exception e)
        {
            _logger.LogInformation("Saga exception {}", e.Message);
            throw new RpcException(new Status(StatusCode.Internal, "Cannot get notification microservice response"));
        }

        return response;
    }

    private async Task SendEmailCommand(Response<NotificationCreatedEvent> response)
    {
        try
        {
            var emailRequest = new SendEmailCommand
            {
                Content = response.Message.Content,
                Email = response.Message.GuestEmail,
                Subject = response.Message.Subject
            };
            var responseEmail = await _emailClient.GetResponse<EmailSendStatus>(emailRequest);
            if (!responseEmail.Message.IsSuccess)
            {
                _logger.LogInformation("Failed email send {}", responseEmail.Message.FailureDetails);
                throw new RpcException(new Status(StatusCode.InvalidArgument, responseEmail.Message.FailureDetails!));
            }

            _logger.LogInformation("Received email status content{}", response.Message.Content);
        }
        catch (System.Exception e)
        {
            _logger.LogInformation("Saga exception {}", e.Message);
            throw new RpcException(new Status(StatusCode.Internal, "Cannot get email service response"));
        }
    }

    private async Task PublishEvent(Handlers.CreateReservationResponse reservation)
    {
        var @event = new CreatedReservationEvent
        {
            ReservationId = reservation.Reservation.Id,
            GuestId = reservation.Reservation.GuestId,
            AccommodationName = reservation.AccommodationResponse.Accommodation.Name,
            AccommodationId =  reservation.AccommodationResponse.Accommodation.Id,
            HostId = new Guid(reservation.AccommodationResponse.Accommodation.HostId),
            From = reservation.Reservation.DateRange.From,
            To = reservation.Reservation.DateRange.To,
            GuestEmail = reservation.Reservation.GuestEmail,
        };
        await _bus.PublishAsync(@event);
    }
}