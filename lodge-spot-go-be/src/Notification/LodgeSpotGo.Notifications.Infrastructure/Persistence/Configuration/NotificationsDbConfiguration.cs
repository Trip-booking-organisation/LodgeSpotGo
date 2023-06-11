using LodgeSpotGo.Notifications.Core.Notifications;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace LodgeSpotGo.Notifications.Infrastructure.Persistence.Configuration;

public class NotificationsDbConfiguration
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<HostNotification>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(x => x.Id).SetIdGenerator(new GuidGenerator());
        });
    }
}