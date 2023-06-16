using LodgeSpotGo.Notifications.Api.Dto.Response;
using LodgeSpotGo.Notifications.Core.Notifications;

namespace LodgeSpotGo.Notifications.Api.Mappers;

public static class NotificationsMapper
{
    public static List<NotificationResponse> MapNotificationsHost(List<HostNotification> hostNotifications)
    {
        return hostNotifications.Select(notification => new NotificationResponse
        {
            Content = notification.Content,
            Type = notification.HostNotificationType.ToString(),
            CreatedAt = notification.CreatedAt
        }).ToList();
    }
}