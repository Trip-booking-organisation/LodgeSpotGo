using Grpc.Net.Client;

namespace JetSetGo.AccommodationManagement.Grpc.Clients;

public class ReservationClient : IReservationClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReservationClient> _logger;

    public ReservationClient(IConfiguration configuration, ILogger<ReservationClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public GetReservationsResponse GetReservationsByGuestAndHostId(Guid guestId, Guid hostId)
    {
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",_configuration["ReservationUrl"]);
        _logger.LogInformation(@"---------------Calling reservation microservice : {}",guestId.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["ReservationUrl"]!);
        var client = new GetReservationApp.GetReservationAppClient(channel);
        var request = new GetReservationByGuestAndAccom()
        {
            GuestId = guestId.ToString(),
            AccommodationId = hostId.ToString()
        };
        try
        {
           
            var reply = client.GetReservationByGuestAndAccomRequest(request);
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