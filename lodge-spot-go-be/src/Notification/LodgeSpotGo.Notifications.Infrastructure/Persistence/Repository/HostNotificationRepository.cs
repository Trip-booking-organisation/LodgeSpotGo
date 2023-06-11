using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Notifications.Infrastructure.Persistence.Settings;
using MongoDB.Driver;

namespace LodgeSpotGo.Notifications.Infrastructure.Persistence.Repository;

public class HostNotificationRepository : IHostNotificationRepository
{
    private readonly IMongoCollection<HostNotification> _notificationsCollection;

    public HostNotificationRepository(DbSettings dbSettings)
    {
        var client = new MongoClient(dbSettings.ConnectionString);
        var mongo = client.GetDatabase(dbSettings.DatabaseName);
        _notificationsCollection = mongo.GetCollection<HostNotification>(
            dbSettings.HostNotificationCollection
            );
    }

    public Task CreateAsync(HostNotification hostNotification) =>
        _notificationsCollection.InsertOneAsync(hostNotification);

    public async Task<List<HostNotification>> GetAllNotificationsByHost(Guid id)=> 
        await _notificationsCollection.
            Find(notification => notification.HostId == id).
            ToListAsync();
}