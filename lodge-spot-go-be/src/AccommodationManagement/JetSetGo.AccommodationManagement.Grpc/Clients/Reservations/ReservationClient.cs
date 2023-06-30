using System.Diagnostics;
using Grpc.Net.Client;

namespace JetSetGo.AccommodationManagement.Grpc.Clients.Reservations;

public class ReservationClient : IReservationClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReservationClient> _logger;
    public const string ServiceName = "ReservationClient";
    public static readonly ActivitySource ActivitySource = new(ServiceName);

    public ReservationClient(IConfiguration configuration, ILogger<ReservationClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public GetReservationsResponse GetReservationsByGuestAndHostId(Guid guestId, Guid hostId)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("GuestId", guestId);
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
            activity?.Stop();
            return reply;
        }
        catch (Exception ex)
        {
            activity?.Stop();
            _logger.LogInformation(@"-------------Couldn't call Reservation microservice: {}", ex.Message);
            return null!;
        }
    }
}