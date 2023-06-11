using LodgeSpotGo.Notifications.Api.Dto.Response;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;

namespace LodgeSpotGo.Notifications.Api.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this WebApplication application)
    {
        application.MapGet("api/v1/host-notifications/{hostId:guid}", GetNotificationGyHost);
    }

    private static async Task<List<HostNotification>> GetNotificationGyHost(
        Guid hostId,
        IHostNotificationRepository hostNotificationRepository) => 
        await hostNotificationRepository.GetAllNotificationsByHost(hostId);
}