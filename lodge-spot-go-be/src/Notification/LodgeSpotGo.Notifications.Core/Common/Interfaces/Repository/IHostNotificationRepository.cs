using LodgeSpotGo.Notifications.Core.Notifications;

namespace LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;

public interface IHostNotificationRepository
{
    Task CreateAsync(HostNotification hostNotification);
    Task<List<HostNotification>> GetAllNotificationsByHost(Guid id);

}