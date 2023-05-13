using AutoMapper;
using Grpc.Net.Client;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchAccommodationClient;

public class SearchAccommodationClient : ISearchAccommodationClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchAccommodationClient> _logger;

    public SearchAccommodationClient(IConfiguration configuration, IMapper mapper, ILogger<SearchAccommodationClient> logger)
    {
        _configuration = configuration;
        _mapper = mapper;
        _logger = logger;
    }
    public GetAccommodationListResponse SearchAccommodation(SearchRequest request)
    {
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",_configuration["AccommodationUrl"]);
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",request.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["AccommodationUrl"]!);
        var client = new SearchAccommodationApp.SearchAccommodationAppClient(channel);

        try
        {
            var reply = client.SearchAccommodations(request);
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