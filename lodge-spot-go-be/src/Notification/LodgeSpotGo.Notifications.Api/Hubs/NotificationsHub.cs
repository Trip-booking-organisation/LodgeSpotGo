using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Hubs;

public class NotificationsHub : Hub
{
    private readonly ILogger<NotificationsHub> _logger;
    private readonly IHostNotificationRepository _notificationRepository;
    private readonly IGuestNotificationRepository _guestNotificationRepository;

    public NotificationsHub(
        ILogger<NotificationsHub> logger, 
        IHostNotificationRepository notificationRepository, 
        IGuestNotificationRepository guestNotificationRepository)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _guestNotificationRepository = guestNotificationRepository;
    }
    public async Task SendNotificationsRequestHost(string userId)
    {
        _logger.LogInformation("-----User id ----- {}",userId);
        var notifications = await _notificationRepository.GetAllNotificationsByHost(new Guid(userId));
        var notificationsMapped = NotificationsMapper.MapNotificationsHost(notifications);
        await Clients.Clients(Context.ConnectionId).SendAsync($"ReceiveNotification/{userId}",notificationsMapped);
    }
    public async Task SendNotificationsRequestGuest(string userId)
    {
        _logger.LogInformation("-----User id ----- {}",userId);
        var notifications = await _guestNotificationRepository.GetAllNotificationsByGuest(new Guid(userId));
        var notificationsMapped = NotificationsMapper.MapNotificationsGuest(notifications);
        await Clients.Clients(Context.ConnectionId).SendAsync($"ReceiveNotificationGuest/{userId}",notificationsMapped);
    }
}