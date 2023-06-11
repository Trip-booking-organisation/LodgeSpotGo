using LodgeSpotGo.Notifications.Core.Notifications;

namespace LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;

public interface IGuestNotificationRepository
{
    Task CreateAsync(GuestNotification hostNotification);
    Task<List<GuestNotification>> GetAllNotificationsByGuest(Guid id);
}