using Grpc.Net.Client;
using JetSetGo.ReservationManagement.Application.Clients;
using JetSetGo.ReservationManagement.Application.Dto.Response;

namespace JetSetGo.ReservationManagement.Grpc.Clients.GetUserClient;

public class GetUserInfosClient : IGetUserClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetUserInfosClient> _logger;

    public GetUserInfosClient(IConfiguration configuration, ILogger<GetUserInfosClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public UserResponse GetUserInfo(Guid id)
    {
        _logger.LogInformation(@"---------------Calling users microservice : {}",_configuration["UsersUrl"]);
        _logger.LogInformation(@"---------------Calling users microservice : {}",id.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["UsersUrl"]!);
        var client = new GetUserKeycloakService.GetUserKeycloakServiceClient(channel);
        try
        {
            var reply = client.GetUserInfo(new GetUserRequestKeycloak
            {
                Id = id.ToString()
            });
            _logger.LogInformation(@"---------------------Users returns : {}",reply.ToString());
            return new UserResponse
            {
                Email = reply.Mail,
                Name = reply.Name,
                LastName = reply.LastName,
                Id = new Guid(reply.Id)
            };
        }
        catch (System.Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call Users microservice: {}", ex.Message);
            return null!;
        }
    }
}