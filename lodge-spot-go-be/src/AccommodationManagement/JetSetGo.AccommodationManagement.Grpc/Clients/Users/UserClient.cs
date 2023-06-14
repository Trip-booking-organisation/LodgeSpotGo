using Grpc.Net.Client;

namespace JetSetGo.AccommodationManagement.Grpc.Clients.Users;

public class UserClient : IUserClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserClient> _logger;

    public UserClient(IConfiguration configuration, ILogger<UserClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public GetUserResponse GetUserById(Guid userId)
    {
        _logger.LogInformation(@"---------------Calling user microservice : {}",_configuration["UserUrl"]);
        _logger.LogInformation(@"---------------Calling user microservice : {}",userId.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["UserUrl"]!);
        var client = new UserApp.UserAppClient(channel);
        var request = new GetUserRequest()
        {
           Id = userId.ToString()
        };
        try
        {
           
            var reply = client.GetUserById(request);
            _logger.LogInformation(@"---------------------User returns : {}",reply.ToString());
            return reply;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call User microservice: {}", ex.Message);
            return null!;
        }
    }
}