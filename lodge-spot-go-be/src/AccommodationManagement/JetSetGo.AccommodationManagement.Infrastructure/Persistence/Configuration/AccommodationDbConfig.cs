using JetSetGo.AccommodationManagement.Domain.Accommodation;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace JetSetGo.AccommodationManagement.Infrastructure.Persistence.Configuration;

public class AccommodationDbConfig
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<Accommodation>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(x => x.Id).SetIdGenerator(new GuidGenerator());
        });
    }
}