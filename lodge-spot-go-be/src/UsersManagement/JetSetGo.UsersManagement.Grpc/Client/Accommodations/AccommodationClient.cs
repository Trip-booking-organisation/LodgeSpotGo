using Grpc.Net.Client;

namespace JetSetGo.UsersManagement.Grpc.Client.Accommodations;

public class AccommodationClient : IAccommodationClient
{
    private readonly ILogger<AccommodationClient> _logger;
    private readonly IConfiguration _configuration;

    public AccommodationClient(ILogger<AccommodationClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public GetAccommodationHostResponse GetAccommodationByHost(Guid hostId)
    {
      
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",_configuration["AccommodationUrl"]);
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",hostId.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["AccommodationUrl"]!);
        var client = new HostAccommodationApp.HostAccommodationAppClient(channel);
        var request = new GetAccommodationHostRequest
        {
            HostId = hostId.ToString()
        };
        try
        {
           
            var reply = client.GetAccommodationsByHost(request);
            _logger.LogInformation(@"---------------------accommodation returns : {}",reply.ToString());
            return reply;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call accommodation microservice: {}", ex.Message);
            return null!;
        }
    }
}