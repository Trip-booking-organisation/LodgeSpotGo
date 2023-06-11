namespace LodgeSpotGo.Notifications.Core.Notifications;

public class GuestNotification
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public string Content { get; set; } = null!;
}