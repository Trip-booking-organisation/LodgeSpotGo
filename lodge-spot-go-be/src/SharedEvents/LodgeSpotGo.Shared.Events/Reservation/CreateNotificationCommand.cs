namespace LodgeSpotGo.Shared.Events.Reservation;

public class CreateNotificationCommand
{
    public Guid ReservationId { get; set; }
    public Guid GuestId { get; set;}
    public string GuestEmail { get; set;} = null!;
    public string AccommodationName { get; set; } = null!;
    public string AccommodationId { get; set; } = null!;
    public Guid HostId { get; set; }
    public DateTime From { get; set; } 
    public DateTime To { get; set; } 
}