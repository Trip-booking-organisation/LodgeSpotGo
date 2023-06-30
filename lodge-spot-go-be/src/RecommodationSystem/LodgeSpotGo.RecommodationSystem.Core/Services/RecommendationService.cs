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

    public async Task<List<Accommodation>> GetRecommendedAccommodations(Guest request)
    {
        List<Guest> similarGuests = await GetSimilarGuests(request);
        List<Accommodation> accommodations = new List<Accommodation>();
        foreach (var guest in similarGuests)
        {
            accommodations.AddRange(await _recommodationRepository.GetGuestsReservedAccommodations(guest.Name));
        }
        return FilterRecommendedAccommodations(accommodations);
    }

    public async Task<List<Accommodation>> GetSimilarGuestsResservedAccommodations()
    {
        return  null;
    }


    public async Task<List<Guest>> GetSimilarGuests(Guest request)
    {
        List<Guest> similarGuests = await _recommodationRepository.GetGuestsByReservedAccommodations(request.Name);
        similarGuests.AddRange(await _recommodationRepository.GetGuestsByGradedAccommodations1(request.Name));
        return FilterSimlarGuests(request,similarGuests);
    }


    private List<Guest> FilterSimlarGuests(Guest request, List<Guest> guests)
    {
        List<Guest> filtered = new List<Guest>();
        foreach (var guest in guests)
        {
            if (guest != request)
            {
                filtered.Add(guest);
            }
        }

        return filtered;
    }
    
    private List<Accommodation> FilterRecommendedAccommodations(List<Accommodation> accommodations)
    {
        List<Accommodation> filtered = new List<Accommodation>();
        foreach (var acc in accommodations)
        {
            if (!CheckIfContains(filtered,acc))
            {
                filtered.Add(acc);
            }
        }

        return filtered;
    }

    private Boolean CheckIfContains(List<Accommodation> accommodations, Accommodation accommodation)
    {
        foreach (var acc in accommodations)
        {
            if (acc.Id == accommodation.Id)
            {
                return true;
            }
        }

        return false;
    }
    
}