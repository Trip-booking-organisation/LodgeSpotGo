using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Consumers;

public class CanceledReservationConsumer: IConsumer<CanceledReservationEvent>
{
    private readonly ILogger<CanceledReservationConsumer> _logger;
    private readonly IHostNotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationsHub> _hubContext;


    public CanceledReservationConsumer(
        ILogger<CanceledReservationConsumer> logger, 
        IHostNotificationRepository notificationRepository, 
        IHubContext<NotificationsHub> hubContext)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<CanceledReservationEvent> context)
    {
        _logger.LogInformation("---------- Canceled Reservation Event Guest------ {}",context.Message.GuestEmail);
        var content = $@"User with email {context.Message.GuestEmail} canceled reservation for accommodation 
        {context.Message.AccommodationName} at date {context.Message.CancelTime}";
        var notification = new HostNotification
        {
            HostId = context.Message.HostId,
            Content = content,
            CanceledReservationDetails = new CanceledReservationDetails
            {
                GuestId = context.Message.GuestId,
                GuestEmail = context.Message.GuestEmail,
                AccommodationName = context.Message.AccommodationName,
                AccommodationId = context.Message.AccommodationId,
                HostId = context.Message.HostId,
                CancelTime = context.Message.CancelTime
            },
            HostNotificationType = HostNotificationType.CanceledReservation,
            CreatedAt = DateTime.Now
        };
        await _notificationRepository.CreateAsync(notification);
        var notifications = await _notificationRepository.GetAllNotificationsByHost(context.Message.HostId);
        var mapped = NotificationsMapper.MapNotificationsHost(notifications);
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", mapped);
    }
}