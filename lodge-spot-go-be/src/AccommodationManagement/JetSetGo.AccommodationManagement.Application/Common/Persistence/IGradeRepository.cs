using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;

namespace JetSetGo.AccommodationManagement.Application.Common.Persistence;

public interface IGradeRepository
{
    Task CreateGrade(Grade grade);
    Task GetByAccommodation(Guid accommodationId);
}