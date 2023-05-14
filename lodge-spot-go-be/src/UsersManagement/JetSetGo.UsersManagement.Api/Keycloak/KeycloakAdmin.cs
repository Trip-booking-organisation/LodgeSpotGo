namespace JetSetGo.UsersManagement.Api.Keycloak;

public class KeycloakAdmin
{
    public const string SectionName = "KeycloakAuth";
    public string AdminUsername { get; set; } = null!;
    public string AdminPassword { get; set; } = null!;
    public string AdminClientId { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
    public string Realm { get; set; } = null!;
}