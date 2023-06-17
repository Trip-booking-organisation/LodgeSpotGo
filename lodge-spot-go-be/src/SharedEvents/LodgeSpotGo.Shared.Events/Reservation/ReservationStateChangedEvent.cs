namespace LodgeSpotGo.Shared.Events.Reservation;

public class ReservationStateChangedEvent
{
    public string NewStatus { get; set; } = string.Empty;
    public Guid GuestId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AccommodationName { get; set; } = null!;
}