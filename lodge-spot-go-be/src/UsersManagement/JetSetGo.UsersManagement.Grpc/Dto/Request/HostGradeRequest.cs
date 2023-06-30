namespace JetSetGo.UsersManagement.Grpc.Dto.Request;

public class HostGradeRequest
{
    public Guid HostId { get; set; }
    public Guid GuestId { get; set; }
    public string GuestEmail { get; set; } = null!;
    public Guid AccomodationId { get; set; }
    public int Number { get; set; }
    
}