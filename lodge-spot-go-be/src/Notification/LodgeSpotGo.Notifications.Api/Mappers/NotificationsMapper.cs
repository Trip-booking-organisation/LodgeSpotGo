using LodgeSpotGo.Notifications.Api.Dto.Response;
using LodgeSpotGo.Notifications.Core.Notifications;

namespace LodgeSpotGo.Notifications.Api.Mappers;

public static class NotificationsMapper
{
    public static List<NotificationResponse> MapNotificationsHost(IEnumerable<HostNotification> hostNotifications)
    {
        return hostNotifications.Select(notification => new NotificationResponse
        {
            Content = notification.Content,
            Type = notification.HostNotificationType.ToString(),
            CreatedAt = notification.CreatedAt
        }).ToList();
    }
    public static List<NotificationResponse> MapNotificationsGuest(IEnumerable<GuestNotification> hostNotifications)
    {
        return hostNotifications.Select(notification => new NotificationResponse
        {
            Content = notification.Content,
            Type = $"Reservation {notification.StatusChangedTo}",
            CreatedAt = notification.CreatedAt
        }).ToList();
    }
}