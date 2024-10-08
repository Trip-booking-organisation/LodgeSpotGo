﻿namespace LodgeSpotGo.Notifications.Core.Notifications;

public class GuestNotification
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string AccommodationName { get; set; } = string.Empty;
    public string StatusChangedTo { get; set; } = string.Empty;
}