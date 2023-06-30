using Grpc.Net.Client;
using JetSetGo.RecommodationSystem.Grpc;

namespace JetSetGo.AccommodationManagement.Grpc.Clients.Recommendation;

public class RecommendationClient:IRecommendationClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RecommendationClient> _logger;

    public RecommendationClient(IConfiguration configuration, ILogger<RecommendationClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public GetRecommodationsResponse GetRecommendations(GetRecommodationReqest request)
    {
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",_configuration["ReservationUrl"]);
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",request.User.Name.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["Recommendation"]!);
        var client = new ReccomodationApp.ReccomodationAppClient(channel);
        
        try
        {
           
            var reply = client.GetRecommodations(request);
            _logger.LogInformation(@"---------------------Reservation returns : {}",reply.ToString());
            return reply;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call Reservation microservice: {}", ex.Message);
            return null!;
        }
    }
}