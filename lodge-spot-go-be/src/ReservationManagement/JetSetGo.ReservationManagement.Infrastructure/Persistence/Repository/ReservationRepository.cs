using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace JetSetGo.ReservationManagement.Infrastructure.Persistence.Repository;
using Microsoft.Extensions;
using MongoDB.Driver;

public class ReservationRepository : IReservationRepository
{
    private readonly IMongoCollection<Reservation> _reservationCollection;
    public ReservationRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var mongo = client.GetDatabase(dbSettings.Value.DatabaseName);
        _reservationCollection = mongo.GetCollection<Reservation>(dbSettings.Value.CollectionName);
    }
    
    
    public async Task<List<Reservation>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _reservationCollection.Find(_=> true).ToListAsync(cancellationToken);

    public async Task CreateAsync(Reservation reservation)
    {
        await _reservationCollection.InsertOneAsync(reservation);
    }
}