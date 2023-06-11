namespace LodgeSpotGo.Notifications.Core.Notifications;

public class HostNotification
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public string GuestEmail { get; set; } = null!;
    public string AccommodationName { get; set; } = null!;
    public Guid HostId { get; set; }
    public string Content { get; set; } = null!;
}