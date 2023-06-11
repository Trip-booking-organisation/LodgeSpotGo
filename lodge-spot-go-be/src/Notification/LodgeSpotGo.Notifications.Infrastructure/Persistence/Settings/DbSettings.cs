namespace LodgeSpotGo.Notifications.Infrastructure.Persistence.Settings;

public class DbSettings
{
    public const string OptionName = "MongoDatabaseSettings";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string HostNotificationCollection { get; set; } = null!;
    public string GuestNotificationCollection { get; set; } = null!;
}