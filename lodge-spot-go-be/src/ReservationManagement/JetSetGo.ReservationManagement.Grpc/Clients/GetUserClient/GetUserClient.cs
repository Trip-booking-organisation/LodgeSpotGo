using Grpc.Net.Client;
using JetSetGo.ReservationManagement.Application.Clients;
using JetSetGo.ReservationManagement.Application.Clients.Responses;
using JetSetGo.ReservationManagement.Application.Dto.Response;

namespace JetSetGo.ReservationManagement.Grpc.Clients.GetUserClient;

public class GetUserClient : IGetUserClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetUserClient> _logger;

    public GetUserClient(IConfiguration configuration, ILogger<GetUserClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public UserResponse GetUserInfo(Guid id)
    {
        _logger.LogInformation(@"---------------Calling users microservice : {}",_configuration["UsersUrl"]);
        _logger.LogInformation(@"---------------Calling users microservice : {}",id.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["UsersUrl"]!);
        var client = new UserApp.UserAppClient(channel);
        var reply = client.GetUserById(new GetUserRequest
            {
                Id = id.ToString()
            });
            _logger.LogInformation(@"---------------------Users returns : {}",reply.ToString());
            return new UserResponse
            {
                Email = reply.User.Mail,
                Name = reply.User.Name,
                LastName = reply.User.LastName,
                Id = new Guid(reply.User.Id)
            };
    }
}