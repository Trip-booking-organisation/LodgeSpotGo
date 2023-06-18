namespace LodgeSpotGo.Shared.Events.Host;

public class OutstandingHostStatusChanged
{
    public Guid HostId { get; set; }
    public bool IsOutstanding { get; set; }
    public DateTime CreatedAt { get; set; }
}