using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;

namespace JetSetGo.ReservationManagement.Domain.Reservation;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid AccommodationId { get; set; }
    public DateRange DateRange { get; set; }
    public ReservationStatus ReservationStatus { get; set; }
    public bool Deleted { get; set; }
}