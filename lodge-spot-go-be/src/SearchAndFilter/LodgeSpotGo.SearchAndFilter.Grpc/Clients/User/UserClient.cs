using Grpc.Net.Client;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.User;

public class UserClient : IUserClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserClient> _logger;

    public UserClient(IConfiguration configuration, ILogger<UserClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public FiletOutstandingHostResponse IsHostOutstanding(Guid hostId)
    {
        _logger.LogInformation(@"---------------Calling RESERVATION microservice : {}",_configuration["UserUrl"]);
        _logger.LogInformation(@"---------------Calling RESERVATION microservice : {}",hostId.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["UserUrl"]!);
        var client = new FilterOutstandingHostApp.FilterOutstandingHostAppClient(channel);
        var request = new FilterOutstandingHostRequest
        {
            HostId = hostId.ToString()
        };
        try
        {
            var reply = client.IsOutstanding(request);
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