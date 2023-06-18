using Grpc.Net.Client;
using JetSetGo.AccommodationManagement.Application.Dto.Response;
using JetSetGo.ReservationManagement.Grpc;

namespace JetSetGo.AccommodationManagement.Grpc.Clients.Users;

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
        _logger.LogInformation(@"---------------Calling users microservice : {}",_configuration["UserUrl"]);
        _logger.LogInformation(@"---------------Calling users microservice : {}",id.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["UserUrl"]!);
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
        catch (Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call Users microservice: {}", ex.Message);
            return null!;
        }
    }
}