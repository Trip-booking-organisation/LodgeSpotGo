using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;

namespace JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;

public class GetReservationsByGuestIdCommandResponse
{
    public Guid Id { get; set; }
    public Guid AccommodationId { get; set; }
    public DateRange DateRange { get; set; } = null!;
    public ReservationStatus ReservationStatus { get; set; }
    public bool Deleted { get; set; }
    public int NumberOfGuests { get; set; }
    public Guid GuestId { get; set; }
}