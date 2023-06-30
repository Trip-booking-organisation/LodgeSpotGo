using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace JetSetGo.AccommodationManagement.Infrastructure.Persistence.Configuration;

public class GradeDbConfig
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<Grade>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(x => x.Id).SetIdGenerator(new GuidGenerator());
        });
    }
}