namespace JetSetGo.UsersManagement.Grpc.Dto;

public class DeleteHostGradeRequest
{
    public Guid gradeId { get; set; }
    public Guid guestId { get; set; }
}