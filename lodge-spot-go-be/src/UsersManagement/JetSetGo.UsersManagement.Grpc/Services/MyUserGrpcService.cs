using Grpc.Net.Client;
using JetSetGo.UserManagement.Grpc;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class MyUserGrpcService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MyUserGrpcService> _logger;

    public MyUserGrpcService(IConfiguration configuration, ILogger<MyUserGrpcService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public ReservationResponse GetReservationResponse(UserRequest request)
    {
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",_configuration["ReservationUrl"]);
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",request.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["ReservationUrl"]!);
        var client = new UserServiceGrpc.UserServiceGrpcClient(channel);

        try
        {
            var reply = client.GetUserReservations(request);
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