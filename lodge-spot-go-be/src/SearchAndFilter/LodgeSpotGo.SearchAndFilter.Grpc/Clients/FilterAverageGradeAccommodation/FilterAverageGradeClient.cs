using Grpc.Net.Client;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.FilterAverageGradeAccommodation;

public class FilterAverageGradeClient : IFilterAverageGradeClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FilterAverageGradeClient> _logger;

    public FilterAverageGradeClient(IConfiguration configuration, ILogger<FilterAverageGradeClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public GetAverageGradeByAccommodationResponse GetAverageGradeForAccommodation(Guid id)
    {
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",_configuration["AccommodationUrl"]);
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",id.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["AccommodationUrl"]!);
        var client = new FilterGradeApp.FilterGradeAppClient(channel);
        var request = new GetAverageGradeByAccommodationRequest
        {
            AccommodationId = id.ToString()
        };
        try
        {
            var reply = client.GetAverageGrade(request);
            _logger.LogInformation(@"---------------------Accommodation returns : {}",reply.ToString());
            return reply;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call Accommodation microservice: {}", ex.Message);
            return null!;
        }
    }
}