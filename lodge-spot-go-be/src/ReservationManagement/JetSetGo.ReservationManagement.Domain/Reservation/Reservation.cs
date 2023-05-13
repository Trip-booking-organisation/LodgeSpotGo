using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;

namespace JetSetGo.ReservationManagement.Domain.Reservation;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid AccommodationId { get; set; }
    public DateRange DateRange { get; set; }
}