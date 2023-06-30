namespace JetSetGo.UsersManagement.Grpc.Dto.Request;

public class CreateTicketInfoRequest
{
    public string SeatNumber { get; set; } = null!;
    public string ContactDetails { get; set; } = null!;
}