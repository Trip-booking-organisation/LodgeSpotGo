using Newtonsoft.Json;

namespace JetSetGo.UsersManagement.Grpc.Keycloak;

public class KeycloakTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = null!;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonProperty("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }
}