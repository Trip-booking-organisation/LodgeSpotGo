namespace LodgeSpotGo.Shared.Events.Notification;

public class NotificationCreatedEvent
{
    public Guid ReservationId { get; set; }
    public string Content { get; set; } = null!;
    public string GuestEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
}