namespace LodgeSpotGo.Notifications.Api.Dto.Response;

public class NotificationResponse
{
    public string Content { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}