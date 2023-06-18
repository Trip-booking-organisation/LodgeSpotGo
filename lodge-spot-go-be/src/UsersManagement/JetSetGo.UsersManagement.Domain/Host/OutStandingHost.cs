namespace JetSetGo.UsersManagement.Domain.Host;

public class OutStandingHost
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public bool IsOutStandingHost { get; set; }
}