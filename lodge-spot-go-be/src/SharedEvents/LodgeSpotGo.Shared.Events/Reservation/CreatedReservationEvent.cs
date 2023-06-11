namespace LodgeSpotGo.Shared.Events.Reservation;

public class CreatedReservationEvent
{
    public Guid GuestId { get; set;}
    public string GuestEmail { get; set; } = null!;
    public string AccommodationName { get; set; } = null!;
}