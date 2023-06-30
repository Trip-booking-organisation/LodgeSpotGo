using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Domain.HostGrade.Entities;
using JetSetGo.UsersManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JetSetGo.UsersManagement.Infrastructure.Persistence.Repository;

public class HostGradeRepository:IHostGradeRepository
{
    private readonly IMongoCollection<HostGrade> _hostGradeCollection;

    public HostGradeRepository(DatabaseSettings dbSettings)
    {
        var client = new MongoClient(dbSettings.ConnectionString);
        var mongo = client.GetDatabase(dbSettings.DatabaseName);
        _hostGradeCollection = mongo.GetCollection<HostGrade>(dbSettings.HostGradeCollectionName);
    }
    
    public async Task CreateGrade(HostGrade grade)
    {
        await _hostGradeCollection.InsertOneAsync(grade);
    }

    public async Task<HostGrade> GetById(Guid id, CancellationToken cancellationToken = default) =>
        await _hostGradeCollection.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task DeleteGrade(Guid id)
    {
        var filter = Builders<HostGrade>.Filter.Eq(g => g.Id, id);
        await _hostGradeCollection.DeleteOneAsync(filter);
    }

    public async Task UpdateGrade(HostGrade grade)
    {
        await _hostGradeCollection.ReplaceOneAsync(g => g.Id == grade.Id, grade);
    }

    public async Task<List<HostGrade>> GetAllByGuest(Guid guestId) =>
        await _hostGradeCollection.Find(g => g.GuestId == guestId).ToListAsync();

    public async Task<List<HostGrade>> GetAllByHost(Guid hostId) =>
        await _hostGradeCollection.Find(g => g.HostId == hostId).ToListAsync();

}