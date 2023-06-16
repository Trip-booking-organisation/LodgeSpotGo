using Grpc.Core;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

namespace JetSetGo.RecommodationSystem.Grpc.Services;

public class RecommodationService : ReccomodationApp.ReccomodationAppBase
{
    private readonly IRecommodationRepository _recommodationRepository;

    public RecommodationService(IRecommodationRepository recommodationRepository)
    {
        _recommodationRepository = recommodationRepository;
    }
    public override async Task<getRecommodationsResponse> GetRecommodations(GetRecommodationReqest request, ServerCallContext context)
    {
        var response = new getRecommodationsResponse();
        var bla = await _recommodationRepository.getRecommodations();
        response.Response = "uspesno kreirano";
        return response;
    }
}