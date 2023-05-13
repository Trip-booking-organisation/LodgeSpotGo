using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;

namespace JetSetGo.ReservationManagement.Domain.Reservation;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid AccommodationId { get; set; }
    public DateRange DateRange { get; set; } = null!;
    public ReservationStatus ReservationStatus { get; set; }
    public bool Deleted { get; set; }

    public bool IsOverlapping(DateRange dateRange)
    {
        if (DateRange.From >= dateRange.From && DateRange.From <= dateRange.To)
            return true;
        if (DateRange.To >= dateRange.From && DateRange.To <= dateRange.To)
            return true;
        if (DateRange.To >= dateRange.From && DateRange.To <= dateRange.To)
            return true;
        if (DateRange.From <= dateRange.From && DateRange.To >= dateRange.To)
            return true;
        return DateRange.From == dateRange.From && DateRange.To == dateRange.To;
    }
}