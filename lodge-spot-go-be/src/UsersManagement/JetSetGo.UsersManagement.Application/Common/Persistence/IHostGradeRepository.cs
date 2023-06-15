using JetSetGo.UsersManagement.Domain.HostGrade.Entities;

namespace JetSetGo.UsersManagement.Application.Common.Persistence;

public interface IHostGradeRepository
{
    Task CreateGrade(HostGrade grade);
    Task<HostGrade> GetById(Guid id, CancellationToken cancellationToken = default);

    Task DeleteGrade(Guid id);
    
    Task UpdateGrade(HostGrade grade);
    
    Task<List<HostGrade>> GetAllByGuest(Guid guestId);
    
    Task<List<HostGrade>> GetAllByHost(Guid hostId);
}