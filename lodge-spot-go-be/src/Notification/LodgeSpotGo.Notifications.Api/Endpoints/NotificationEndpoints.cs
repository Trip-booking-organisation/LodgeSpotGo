using LodgeSpotGo.Notifications.Api.Dto.Response;
using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this WebApplication application)
    {
        application.MapGet("api/v1/host-notifications/{hostId:guid}", GetNotificationGyHost);
    }

    private static async Task<IResult> GetNotificationGyHost(
        Guid hostId,
        IHostNotificationRepository hostNotificationRepository) {
        var notifications = await 
            hostNotificationRepository.GetAllNotificationsByHost(hostId);
        return Results.Ok(notifications);
    }
}