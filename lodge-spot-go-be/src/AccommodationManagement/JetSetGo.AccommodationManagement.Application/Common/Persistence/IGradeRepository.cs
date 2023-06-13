using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;

namespace JetSetGo.AccommodationManagement.Application.Common.Persistence;

public interface IGradeRepository
{
    Task CreateGrade(Grade grade);
    Task<List<Grade>> GetByAccommodation(Guid accommodationId);
    Task<Grade> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<List<Grade>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateGrade(Grade grade);
    Task DeleteGrade(Guid id);
}