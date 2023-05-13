using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;

namespace JetSetGo.ReservationManagement.Application.SearchReservations;

public class SearchReservationResponse
{
    public Guid Id { get; set; }
    public Guid AccommodationId { get; set; }
    public DateRange DateRange { get; set; } = null!;
}