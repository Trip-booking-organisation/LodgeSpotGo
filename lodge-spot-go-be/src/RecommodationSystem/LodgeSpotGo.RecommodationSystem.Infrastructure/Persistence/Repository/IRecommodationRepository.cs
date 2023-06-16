using LodgeSpotGo.RecommodationSystem.Core.Model;

namespace LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

public interface IRecommodationRepository
{
    Task<bool> getRecommodations();
    Task<Guest> CreateGuest(Guest request);
    Task<Accommodation> CreateAccommodation(Accommodation request);
    

}