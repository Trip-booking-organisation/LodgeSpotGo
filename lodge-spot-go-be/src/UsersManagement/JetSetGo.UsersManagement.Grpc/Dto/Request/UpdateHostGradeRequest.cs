namespace JetSetGo.UsersManagement.Grpc.Dto;

public class UpdateHostGradeRequest
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public Guid GuestId { get; set; }
    public int Number { get; set; }
}