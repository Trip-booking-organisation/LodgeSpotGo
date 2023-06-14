namespace JetSetGo.UsersManagement.Grpc.Dto;

public class HostGradeRequest
{
    public Guid HostId { get; set; }
    public Guid GuestId { get; set; }
    public Guid AccomodationId { get; set; }
    public int Number { get; set; }
    
}