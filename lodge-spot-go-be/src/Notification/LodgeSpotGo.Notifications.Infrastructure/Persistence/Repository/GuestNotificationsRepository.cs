using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Notifications.Infrastructure.Persistence.Settings;
using MongoDB.Driver;

namespace LodgeSpotGo.Notifications.Infrastructure.Persistence.Repository;

public class GuestNotificationsRepository : IGuestNotificationRepository
{
    private readonly IMongoCollection<GuestNotification> _notificationsCollection;

    public GuestNotificationsRepository(DbSettings dbSettings)
    {
        var client = new MongoClient(dbSettings.ConnectionString);
        var mongo = client.GetDatabase(dbSettings.DatabaseName);
        _notificationsCollection = mongo.GetCollection<GuestNotification>(
            dbSettings.GuestNotificationCollection
        );
    }
    public Task CreateAsync(GuestNotification hostNotification) =>  
        _notificationsCollection.InsertOneAsync(hostNotification);

    public async Task<List<GuestNotification>> GetAllNotificationsByGuest(Guid id)=> 
        await _notificationsCollection.
            Find(notification => notification.GuestId == id).
            ToListAsync();
}