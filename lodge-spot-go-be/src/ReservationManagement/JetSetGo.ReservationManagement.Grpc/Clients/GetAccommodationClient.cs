using Grpc.Net.Client;

namespace JetSetGo.ReservationManagement.Grpc.Clients;

public class GetAccommodationClient : IGetAccommodationClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetAccommodationClient> _logger;

    public GetAccommodationClient(IConfiguration configuration, ILogger<GetAccommodationClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public GetAccommodationResponse GetAccommodation(GetAccommodationRequest request)
    {
        
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",_configuration["AccommodationUrl"]);
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",request.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["AccommodationUrl"]!);
        var client = new GetAccommodationApp.GetAccommodationAppClient(channel);

        try
        {
            var reply = client.GetAccommodation(request);
            _logger.LogInformation(@"---------------------Accommodation returns : {}",reply.ToString());
            return reply;
        }
        catch (System.Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't process Accommodation microservice result: {}", ex.Message);
            return null!;
        }
    }
}