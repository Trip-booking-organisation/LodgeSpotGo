using LodgeSpotGo.RecommodationSystem.Core.Model;

namespace LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

public interface IRecommodationRepository
{
    Task<Boolean> getRecommodations();

}