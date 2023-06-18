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

    public RecommodationService(RecommendationService recommendationService, IMapper mapper)
    {
        _recommendationService = recommendationService;
        _mapper = mapper;
    }

    public override async Task<GetRecommodationsResponse> GetRecommodations(GetRecommodationReqest request, ServerCallContext context)
    {
        var guest = new Guest
        {
            Name = request.User.Name
        };

        var accommodations = await _recommendationService.GetRecommendedAccommodations(guest);
        return new GetRecommodationsResponse()
        {
            Response = { accommodations.Select(x => _mapper.Map<RecAccommodationResponse>(x)).ToList() }
        };
        

    }
}