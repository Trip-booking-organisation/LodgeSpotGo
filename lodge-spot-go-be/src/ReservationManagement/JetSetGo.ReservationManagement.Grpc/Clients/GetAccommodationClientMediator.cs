using AutoMapper;
using Grpc.Net.Client;
using JetSetGo.ReservationManagement.Application.Clients;
using JetSetGo.ReservationManagement.Application.Clients.Responses;

namespace JetSetGo.ReservationManagement.Grpc.Clients;

public class GetAccommodationClientMediator : IClientAccommodationMediator
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetAccommodationClientMediator> _logger;
    private readonly IMapper _mapper;

    public GetAccommodationClientMediator(IConfiguration configuration, ILogger<GetAccommodationClientMediator> logger, IMapper mapper)
    {
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
    }

    public AccommodationDtoResponse GetAccommodation(Guid id)
    {
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",_configuration["AccommodationUrl"]);
        _logger.LogInformation(@"---------------Calling accommodation microservice : {}",id.ToString());
        var channel = GrpcChannel.ForAddress(_configuration["AccommodationUrl"]!);
        var client = new GetAccommodationApp.GetAccommodationAppClient(channel);

        try
        {
            var reply = client.GetAccommodation(new GetAccommodationRequest{Id = id.ToString()});
            _logger.LogInformation(@"---------------------Accommodation returns : {}",reply.ToString());
            return _mapper.Map<AccommodationDtoResponse>(reply);
        }
        catch (System.Exception ex)
        {
            _logger.LogInformation(@"-------------Couldn't call Accommodation microservice: {}", ex.Message);
            return null!;
        }
    }
}