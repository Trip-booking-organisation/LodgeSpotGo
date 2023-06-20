using AutoMapper;
using Grpc.Core;
using LodgeSpotGo.RecommodationSystem.Core.Model;
using LodgeSpotGo.RecommodationSystem.Core.Services;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;
using MassTransit.Initializers;

namespace JetSetGo.RecommodationSystem.Grpc.Services;

public class RecommodationService : ReccomodationApp.ReccomodationAppBase
{
    private readonly RecommendationService _recommendationService;
    private readonly IMapper _mapper;
    public const string ServiceName = "RecommodationService";
    public static readonly ActivitySource ActivitySource = new("Recommendation activity");

    public RecommodationService(RecommendationService recommendationService, IMapper mapper)
    {
        _recommendationService = recommendationService;
        _mapper = mapper;
    }

    public override async Task<GetRecommodationsResponse> GetRecommodations(GetRecommodationReqest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("UserName", request.User.Name);
        var guest = new Guest
        {
            Name = request.User.Name
        };

        var guests = await _recommendationService.GetRecommendedAccommodations(guest);
        activity?.Stop();
        return new getRecommodationsResponse()
        var accommodations = await _recommendationService.GetRecommendedAccommodations(guest);
        return new GetRecommodationsResponse()
        {
            Response = { accommodations.Select(x => _mapper.Map<RecAccommodationResponse>(x)).ToList() }
        };
        

    }
}