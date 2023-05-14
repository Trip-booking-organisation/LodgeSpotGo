using Grpc.Net.Client;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchReservationClient;

public class SearchReservationClient : ISearchReservationClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SearchReservationClient> _logger;

    public SearchReservationClient(ILogger<SearchReservationClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public GetReservationListResponse SearchReservations(ReservationSearchRequest request)
    {
        _logger.LogInformation(@"---------------Calling RESERVATION microservice : {}",_configuration["ReservationUrl"]);
        _logger.LogInformation(@"---------------Calling RESERVATION microservice : {}",request.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["ReservationUrl"]!);
        var client = new SearchReservationApp.SearchReservationAppClient(channel);

        try
        {
            var reply = client.SearchReservations(request);
            _logger.LogInformation(@"---------------------RESERVATION returns : {}",reply.ToString());
            return reply;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call RESERVATION microservice: {}", ex.Message);
            return null!;
        }
    }
}