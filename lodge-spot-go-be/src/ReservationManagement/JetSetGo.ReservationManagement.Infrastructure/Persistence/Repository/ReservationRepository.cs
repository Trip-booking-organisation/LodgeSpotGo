using JetSetGo.ReservationManagement.Application.CancelReservation;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
namespace JetSetGo.ReservationManagement.Infrastructure.Persistence.Repository;
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

    public async Task<List<Reservation>> SearchReservations(SearchReservationsQuery request) =>
        await _reservationCollection.Find(x =>
                x.DateRange.From.CompareTo(request.StartDate) <= 0
                && x.DateRange.To.CompareTo(request.EndDate) >= 0)
            .ToListAsync();

    public async Task CancelReservation(Reservation request)
    {
        var filter = Builders<Reservation>.Filter.Eq(r => r.Id, request.Id);
        var update = Builders<Reservation>.Update
            .Set(r => r.Deleted, true);
        await _reservationCollection.UpdateOneAsync(filter,update);

    }

    public async Task<Reservation> GetById(Guid id,CancellationToken cancellationToken= default) =>
        await _reservationCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task UpdateReservationStatus(Reservation reservation)
    {
        var filter = Builders<Reservation>.Filter.Eq(r => r.Id, reservation.Id);
        var update = Builders<Reservation>.Update
            .Set(r => r.ReservationStatus, reservation.ReservationStatus);
        await _reservationCollection.UpdateOneAsync(filter,update);
    }

    public async Task<List<Reservation>> GetByAccommodationId(Reservation reservation) =>
        await _reservationCollection.Find(x =>
            x.AccommodationId == reservation.AccommodationId
            && x.Id != reservation.Id).ToListAsync();

    public async Task<List<Reservation>> GetByGuestId(Guid guestId) =>
        await _reservationCollection.Find(x =>
            x.GuestId == guestId)
            .ToListAsync();
}