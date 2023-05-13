using JetSetGo.ReservationManagement.Domain.Reservation;

namespace JetSetGo.ReservationManagement.Application.Common.Persistence;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task  CreateAsync(Reservation reservation);
}