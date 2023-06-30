using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Shared.Events.Notification;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Consumers;

public class CreateReservationCommandConsumer : IConsumer<CreateNotificationCommand>
{
    private readonly ILogger<CreateReservationCommandConsumer> _logger;
    private readonly IHostNotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public CreateReservationCommandConsumer(
        ILogger<CreateReservationCommandConsumer> logger, 
        IHostNotificationRepository notificationRepository, 
        IHubContext<NotificationsHub> hubContext)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<CreateNotificationCommand> context)
    {
        _logger.LogInformation("---------- Created Reservation Event Guest------ {}", context.Message.GuestEmail);
        var content = $@"User with email {context.Message.GuestEmail} made reservation for accommodation 
        {context.Message.AccommodationName} from date: {context.Message.From} to date: {context.Message.To}.";
        var emailContent = $@"You have successfully submitted reservation for accommodation 
        {context.Message.AccommodationName} from date: {context.Message.From} to date: {context.Message.To}!.Please wait for host approval";
        var notification = new HostNotification
        {
            HostId = context.Message.HostId,
            Content = content,
            CreateReservationDetails = new CreatedReservationDetails
            {
                GuestId = context.Message.GuestId,
                GuestEmail = context.Message.GuestEmail,
                AccommodationName = context.Message.AccommodationName,
                AccommodationId = context.Message.AccommodationId,
                From = context.Message.From,
                To = context.Message.To
            },
            HostNotificationType = HostNotificationType.CreatedReservation,
            CreatedAt = DateTime.Now
        };
        await _notificationRepository.CreateAsync(notification);
        var @event = new NotificationCreatedEvent
        {
            ReservationId = context.Message.ReservationId,
            Content = emailContent,
            GuestEmail = context.Message.GuestEmail,
            Subject = "You are successfully created reservation"
        };
        await context.RespondAsync(@event);
        _logger.LogInformation("---------- Sending notification event ------ {}", @event.ReservationId);
        var notifications = await _notificationRepository.GetAllNotificationsByHost(context.Message.HostId);
        var mapped = NotificationsMapper.MapNotificationsHost(notifications);
        await _hubContext.Clients.All.SendAsync($"ReceiveNotification/{context.Message.HostId}", mapped);
    }
}