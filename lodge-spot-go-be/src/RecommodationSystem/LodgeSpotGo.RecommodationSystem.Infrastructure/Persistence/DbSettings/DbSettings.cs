namespace LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence;

public class DbSettings
{
    public const string SectionName = "ConnectionStrings";
    public string Neo4jDb { get; set; }
    public string DbName { get; set; }
    public string DbPassword { get; set; }

}