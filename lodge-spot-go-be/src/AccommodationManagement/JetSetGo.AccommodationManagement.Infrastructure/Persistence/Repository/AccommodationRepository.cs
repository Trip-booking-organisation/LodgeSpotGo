using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodation;
using JetSetGo.AccommodationManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JetSetGo.AccommodationManagement.Infrastructure.Persistence.Repository;

public class AccommodationRepository: IAccommodationRepository
{
    private readonly IMongoCollection<Accommodation> _accommodationCollection;

    public AccommodationRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var mongo = client.GetDatabase(dbSettings.Value.DatabaseName);
        _accommodationCollection = mongo.GetCollection<Accommodation>(dbSettings.Value.AccommodationCollectionName);
    }

    public async Task<List<Accommodation>> GetAllAsync(CancellationToken cancellationToken= default) =>
        await _accommodationCollection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Accommodation> GetAsync(Guid id,CancellationToken cancellationToken= default) =>
        await _accommodationCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task CreateAsync(Accommodation accommodation) =>
        await _accommodationCollection.InsertOneAsync(accommodation);

    public async Task UpdateAsync(Accommodation accommodation) =>
        await _accommodationCollection.ReplaceOneAsync(x => x.Id == accommodation.Id, accommodation);

    public async Task RemoveAsync(Guid id) =>
        await _accommodationCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<List<Accommodation>> SearchAccommodations(SearchAccommodationQuery request) =>
        await _accommodationCollection.Find(x => 
           x.Address.Country == request.Country 
           && x.Address.City == request.City
           && x.MaxGuests >= request.NumberOfGuests
           && x.MinGuests <= request.NumberOfGuests)
            .ToListAsync();

    public async Task<List<Accommodation>> GetByHost(Guid hostId) =>
        await _accommodationCollection.Find(x =>
                x.HostId == hostId)
            .ToListAsync();
}