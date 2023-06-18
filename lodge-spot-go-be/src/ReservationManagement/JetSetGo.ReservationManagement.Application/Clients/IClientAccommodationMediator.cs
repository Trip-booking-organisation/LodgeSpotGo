using JetSetGo.ReservationManagement.Application.Clients.Responses;

namespace JetSetGo.ReservationManagement.Application.Clients;

public interface IClientAccommodationMediator
{
    AccommodationDtoResponse GetAccommodation(Guid id); 
}