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

    public async Task<List<Grade>> GetByAccommodation(Guid accommodationId) =>
        await _gradeCollection.Find(x => x.AccommodationId == accommodationId)
            .ToListAsync();

    public async Task<Grade> GetById(Guid id, CancellationToken token = default) => 
        await _gradeCollection.Find(x => x.Id == id)
            .FirstOrDefaultAsync(token);

    public async Task<List<Grade>> GetAllAsync(CancellationToken cancellationToken=default) =>
        await _gradeCollection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task UpdateGrade(Grade grade) =>
        await _gradeCollection.ReplaceOneAsync(x => x.Id == grade.Id, grade);

    public async Task DeleteGrade(Guid id)
    {
        var filter = Builders<Grade>.Filter.Eq(r => r.Id, id);
        await _gradeCollection.DeleteOneAsync(filter);
    }
}