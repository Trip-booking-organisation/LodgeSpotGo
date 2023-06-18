using JetSetGo.UsersManagement.Grpc.Dto.Request;
using JetSetGo.UsersManagement.Grpc.Dto.Response;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class JetSetGoService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public JetSetGoService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<CreateTicketResponse> BuyTickets(CreateTicketRequest request)
    {
        var url = _configuration["JetSetGoUrl"];
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {request.Token}");
        var createTicket = new JetSetGoCreateTicketRequest
        {
            PassengerId = request.PassengerId,
            FlightId = request.FlightId,
            NewTickets = request.NewTickets
        };
        var response = await _httpClient.PostAsJsonAsync(url,createTicket);
        var result = new CreateTicketResponse
        {
            Success = false
        };
        if (response.IsSuccessStatusCode)
            result.Success = true;
        return result;
    }
}