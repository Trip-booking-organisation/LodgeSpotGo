namespace JetSetGo.UsersManagement.Grpc.Dto.Request;

public class JetSetGoCreateTicketRequest
{
    public Guid FlightId { get; set; }
    public Guid PassengerId { get; set; }
    public List<CreateTicketInfoRequest> NewTickets { get; set; } = null!;
}