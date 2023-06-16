using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Hubs;

public class NotificationsHub : Hub
{
    private readonly ILogger<NotificationsHub> _logger;
    private readonly IHostNotificationRepository _notificationRepository;

    public NotificationsHub(ILogger<NotificationsHub> logger, IHostNotificationRepository notificationRepository)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
    }
    public async Task SendNotificationsRequest(string userId)
    {
        _logger.LogInformation("-----User id ----- {}",userId);
        var notifications = await _notificationRepository.GetAllNotificationsByHost(new Guid(userId));
        var notificationsMapped = NotificationsMapper.MapNotificationsHost(notifications);
        await Clients.Clients(Context.ConnectionId).SendAsync("ReceiveNotification",notificationsMapped);
    }
}