using JetSetGo.ReservationManagement.Application.CancelReservation;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Domain.Reservation;

namespace JetSetGo.ReservationManagement.Application.Common.Persistence;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task  CreateAsync(Reservation reservation);
    Task<List<Reservation>> SearchReservations(SearchReservationsQuery request);
    Task CancelReservation(Reservation request);
    Task<Reservation> GetById(Guid id,CancellationToken cancellationToken);
    Task UpdateReservationStatus(Reservation reservation);
    Task<List<Reservation>> GetByAccommodationId(Reservation reservation);
}