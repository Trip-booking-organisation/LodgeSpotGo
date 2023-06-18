namespace JetSetGo.UsersManagement.Grpc.Dto.Request;

public class OutstandingHostDto
{
    public Guid HostId { get; set;}
    public string HostEmail { get; set; } = null!;
    public bool CurrentStatus { get; set; }
}