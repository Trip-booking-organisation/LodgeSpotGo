namespace LodgeSpotGo.Notifications.Core.Notifications;

public class CreatedReservationDetails
{
    public Guid GuestId { get; set;}
    public string GuestEmail { get; set;} = null!;
    public string AccommodationName { get; set; } = null!;
    public string AccommodationId { get; set; } = null!;
    public DateTime From { get; set; } 
    public DateTime To { get; set; } 
}