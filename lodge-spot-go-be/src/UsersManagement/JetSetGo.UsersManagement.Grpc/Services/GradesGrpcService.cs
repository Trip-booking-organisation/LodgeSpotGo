using Grpc.Net.Client;
using JetSetGo.UserManagement.Grpc;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class GradesGrpcService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GradesGrpcService> _logger;

    public GradesGrpcService(IConfiguration configuration, ILogger<GradesGrpcService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public CreateHostGradeResponse GetReservationResponse(CreateHostGradeRequest request)
    {
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",_configuration["ReservationUrl"]);
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",request);
        var channel = GrpcChannel.ForAddress(_configuration["ReservationUrl"]!);
        var client = new GradeApp.GradeAppClient(channel);

        try
        {
            var reply = client.CreateGradeForHost(request);
            _logger.LogInformation(@"---------------------Reservation returns : {}",reply);
            return reply;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call Reservation microservice: {}", ex.Message);
            return null!;
        }
    }
}