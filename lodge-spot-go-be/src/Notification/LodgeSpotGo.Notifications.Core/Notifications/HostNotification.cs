namespace LodgeSpotGo.Notifications.Core.Notifications;

public class HostNotification
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public string Content { get; set; } = null!;
    public CreatedReservationDetails? CreateReservationDetails { get; set; }
    public CanceledReservationDetails? CanceledReservationDetails { get; set; }
    public HostNotificationType HostNotificationType { get; set; }
    public DateTime CreatedAt { get; set; }
}