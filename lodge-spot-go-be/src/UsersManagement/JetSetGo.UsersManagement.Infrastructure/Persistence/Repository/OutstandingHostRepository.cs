using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Domain.Host;
using JetSetGo.UsersManagement.Domain.HostGrade.Entities;
using JetSetGo.UsersManagement.Infrastructure.Persistence.Settings;
using MongoDB.Driver;

namespace JetSetGo.UsersManagement.Infrastructure.Persistence.Repository;

public class OutstandingHostRepository:IOutstandingHostRepository
{
    private readonly IMongoCollection<OutStandingHost> _collection;

    public OutstandingHostRepository(DatabaseSettings dbSettings)
    {
        var client = new MongoClient(dbSettings.ConnectionString);
        var mongo = client.GetDatabase(dbSettings.DatabaseName);
        _collection = mongo.GetCollection<OutStandingHost>(dbSettings.OutStandingHostCollectionName);
    }

    public Task CreateOutStandingHost(OutStandingHost host) => 
        _collection.InsertOneAsync(host);
    public OutStandingHost? GetByHostId(Guid id, CancellationToken cancellationToken = default) => 
        _collection.Find(x => x.HostId == id).SingleOrDefault(cancellationToken);
    public Task Update(OutStandingHost host) => 
        _collection.ReplaceOneAsync(h => h.HostId == host.HostId, host);
}