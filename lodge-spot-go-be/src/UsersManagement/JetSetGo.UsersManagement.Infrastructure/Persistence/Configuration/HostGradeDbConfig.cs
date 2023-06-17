using JetSetGo.UsersManagement.Domain.Host;
using JetSetGo.UsersManagement.Domain.HostGrade.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace JetSetGo.UsersManagement.Infrastructure.Persistence.Configuration;

public class HostGradeDbConfig
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<HostGrade>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(x => x.Id).SetIdGenerator(new GuidGenerator());
        });
        BsonClassMap.RegisterClassMap<OutStandingHost>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(x => x.Id).SetIdGenerator(new GuidGenerator());
        });
    }
}