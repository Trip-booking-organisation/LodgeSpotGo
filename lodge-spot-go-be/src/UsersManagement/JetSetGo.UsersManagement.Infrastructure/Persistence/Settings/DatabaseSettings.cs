namespace JetSetGo.UsersManagement.Infrastructure.Persistence.Settings;

public class DatabaseSettings
{
    public const string OptionName = "MongoDatabaseSettings";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string HostGradeCollectionName { get; set; } = null!;
    
}