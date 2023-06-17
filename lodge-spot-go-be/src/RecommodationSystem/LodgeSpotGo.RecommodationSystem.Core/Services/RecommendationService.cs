using System.Runtime.CompilerServices;
using LodgeSpotGo.RecommodationSystem.Core.Model;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

namespace LodgeSpotGo.RecommodationSystem.Core.Services;

public class RecommendationService
{
    private readonly IRecommodationRepository _recommodationRepository;

    public RecommendationService(IRecommodationRepository recommodationRepository)
    {
        _recommodationRepository = recommodationRepository;
    }

    public async Task<bool> MakeReservation(Guest guest, Accommodation accommodation)
    {
        var exists = await _recommodationRepository.GetGuestByMail(guest.Name);
        if (exists is null)
        {
            await _recommodationRepository.CreateGuest(guest);
        }
        
        var accommodationExist = await _recommodationRepository.GetAccommodationById(accommodation.Id);
        if (accommodationExist is null)
        {
            await _recommodationRepository.CreateAccommodation(accommodation);
        }

        return await _recommodationRepository.MakeReservation(guest, accommodation.Id);
    }

    public async Task MakeAccommodationGrade(int grade, Guest guest, Accommodation accommodation)
    {
        var exists = await _recommodationRepository.GetGuestByMail(guest.Name);
        if (exists is null)
        {
            await _recommodationRepository.CreateGuest(guest);
        }
        
        var accommodationExist = await _recommodationRepository.GetAccommodationById(accommodation.Id);
        if (accommodationExist is null)
        {
            await _recommodationRepository.CreateAccommodation(accommodation);
        }

        await _recommodationRepository.MakeAccommodationGrade(guest, accommodation.Id,grade);
    }

    public async Task<Accommodation> CreateAccommodation(Accommodation request)
    {
        return await _recommodationRepository.CreateAccommodation(request);
    }
    
    public async Task<Guest> CreateGuest(Guest request)
    {
        return await _recommodationRepository.CreateGuest(request);
    }

    public async Task<List<Guest>> GetRecommendedAccommodations(Guest request)
    {
        return await _recommodationRepository.GetGuestsByGradedAccommodations(request.Name);
    }

  
    
    
}