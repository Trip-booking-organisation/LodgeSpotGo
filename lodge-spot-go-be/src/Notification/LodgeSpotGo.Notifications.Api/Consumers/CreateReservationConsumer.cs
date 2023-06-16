using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Consumers;

public class CreateReservationConsumer : IConsumer<CreatedReservationEvent>
{
    private readonly ILogger<CreateReservationConsumer> _logger;
    private readonly IHostNotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public CreateReservationConsumer(
        ILogger<CreateReservationConsumer> logger, 
        IHostNotificationRepository notificationRepository, 
        IHubContext<NotificationsHub> hubContext)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<CreatedReservationEvent> context)
    {
        _logger.LogInformation("---------- Created Reservation Event Guest------ {}", context.Message.GuestEmail);
        var content = $@"User with email {context.Message.GuestEmail} made reservation for accommodation 
        {context.Message.AccommodationName} from date: {context.Message.From} to date: {context.Message.To}.";
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
        var notifications = await _notificationRepository.GetAllNotificationsByHost(context.Message.HostId);
        var mapped = NotificationsMapper.MapNotificationsHost(notifications);
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", mapped);
    }
}