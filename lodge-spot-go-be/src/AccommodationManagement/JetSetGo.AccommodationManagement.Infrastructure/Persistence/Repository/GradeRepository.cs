using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;
using JetSetGo.AccommodationManagement.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JetSetGo.AccommodationManagement.Infrastructure.Persistence.Repository;

public class GradeRepository : IGradeRepository
{
    private readonly IMongoCollection<Grade> _gradeCollection;

    public GradeRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var mongo = client.GetDatabase(dbSettings.Value.DatabaseName);
        _gradeCollection = mongo.GetCollection<Grade>(dbSettings.Value.GradeCollectionName);
    }

    public async Task CreateGrade(Grade grade) =>
        await _gradeCollection.InsertOneAsync(grade);

    public async Task GetByAccommodation(Guid accommodationId) =>
        await _gradeCollection.FindAsync(x => x.AccommodationId == accommodationId);
}