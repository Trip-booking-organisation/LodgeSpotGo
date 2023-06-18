using LodgeSpotGo.RecommodationSystem.Core.Model;

namespace LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

public interface IRecommodationRepository
{
    Task<bool> getRecommodations();
    Task<Guest> CreateGuest(Guest request);
    Task<Accommodation> CreateAccommodation(Accommodation request);
    Task<bool> MakeReservation(Guest guest, string accommodationId);
    Task<Guest?> GetGuestByMail(string mail);

    Task<Accommodation?> GetAccommodationById(string id);
    
    Task<bool> MakeAccommodationGrade(Guest guest, string accommodationId,int grade);
    
    Task<List<Guest>> GetGuestsByReservedAccommodations(string guestName);
    Task<List<Guest>> GetGuestsByGradedAccommodations(string guestName);

}