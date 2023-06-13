using JetSetGo.UsersManagement.Domain.HostGrade.Entities;

namespace JetSetGo.UsersManagement.Application.Common.Persistence;

public interface IHostGradeRepository
{
    Task CreateGrade(HostGrade grade);
}