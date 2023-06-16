using Grpc.Core;
using LodgeSpotGo.RecommodationSystem.Core.Model;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

namespace JetSetGo.RecommodationSystem.Grpc.Services;

public class AccommodationService : AccommodationApp.AccommodationAppBase
{
    
    private readonly IRecommodationRepository _recommodationRepository;

    public AccommodationService(IRecommodationRepository recommodationRepository)
    {
        _recommodationRepository = recommodationRepository;
    }
    public override async Task<CreateAccommodationResponse> CreateAccommodation(CreateAccommodationRequest request, ServerCallContext context)
    {
        Accommodation accommodation = new Accommodation
        {
            Name = request.Accommodation.Name,
            Id = request.Accommodation.Id
        };
        var response = await _recommodationRepository.CreateAccommodation(accommodation);
        
        return new CreateAccommodationResponse
        {
            Accommodation = new AccommodationDto
            {
                Id = response.Id,
                Name = response.Name
            }
            
        };
    }
}