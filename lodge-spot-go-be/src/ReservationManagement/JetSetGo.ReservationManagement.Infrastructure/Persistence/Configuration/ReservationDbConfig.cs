using JetSetGo.ReservationManagement.Domain.Reservation;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace JetSetGo.ReservationManagement.Infrastructure.Persistence.Configuration;

public class ReservationDbConfig
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<Reservation>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
        });
    }
}