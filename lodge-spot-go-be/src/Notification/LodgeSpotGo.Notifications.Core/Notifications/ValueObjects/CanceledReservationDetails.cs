namespace LodgeSpotGo.Notifications.Core.Notifications.ValueObjects;

public class CanceledReservationDetails
{
    public Guid GuestId { get; set;}
    public string GuestEmail { get; set;} = null!;
    public string AccommodationName { get; set; } = null!;
    public string AccommodationId { get; set; } = null!;
    public Guid HostId { get; set; }
    public DateTime CancelTime { get; set; }
}