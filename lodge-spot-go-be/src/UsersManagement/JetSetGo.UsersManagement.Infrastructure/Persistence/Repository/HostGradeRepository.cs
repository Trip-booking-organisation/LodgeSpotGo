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
}