using JetSetGo.UsersManagement.Domain.Host;

namespace JetSetGo.UsersManagement.Application.Common.Persistence;

public interface IOutstandingHostRepository
{
    Task CreateOutStandingHost(OutStandingHost host);
    OutStandingHost? GetByHostId(Guid id, CancellationToken cancellationToken = default);
    public Task Update(OutStandingHost host);
}