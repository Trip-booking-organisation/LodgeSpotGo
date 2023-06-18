using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Consumers;

public class ReservationStateChangedConsumer: IConsumer<ReservationStateChangedEvent>
{
    private readonly IGuestNotificationRepository _guestNotificationRepository;
    private readonly ILogger<ReservationStateChangedConsumer> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public ReservationStateChangedConsumer(
        IGuestNotificationRepository guestNotificationRepository, 
        ILogger<ReservationStateChangedConsumer> logger, 
        IHubContext<NotificationsHub> hubContext)
    {
        _guestNotificationRepository = guestNotificationRepository;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<ReservationStateChangedEvent> context)
    {
        _logger.LogInformation(@"Message successfully delivered {}",context.Message.ToString());
        _logger.LogInformation("---------- Canceled Reservation Event Guest------ {}",context.Message.GuestId);
        var content = $@"Your reservation from {context.Message.From} to {context.Message.To} for accommodation 
{context.Message.AccommodationName} changed status to {context.Message.NewStatus} at {context.Message.CreatedAt}.";
        var notification = new GuestNotification
        {
            GuestId = context.Message.GuestId,
            Content = content,
            CreatedAt = context.Message.CreatedAt,
            AccommodationName = context.Message.AccommodationName,
            StatusChangedTo = context.Message.NewStatus
        };
        await _guestNotificationRepository.CreateAsync(notification);
        var notifications = await _guestNotificationRepository.GetAllNotificationsByGuest(context.Message.GuestId);
        var mapped = NotificationsMapper.MapNotificationsGuest(notifications);
        await _hubContext.Clients.All.SendAsync($"ReceiveNotification/{context.Message.GuestId}", mapped);
    }
}