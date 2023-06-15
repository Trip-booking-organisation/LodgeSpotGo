using Newtonsoft.Json;

namespace JetSetGo.UsersManagement.Grpc.Keycloak;

public class KeycloakUserResponse
{
    [JsonProperty("id")]
    public string Id { get; set; } = null!;
    [JsonProperty("username")]
    public string UserName { get; set; } = null!;

    [JsonProperty("firstName")]
    public string FirstName { get; set; } = null!;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = null!;

    [JsonProperty("email")]
    public string Email { get; set; } = null!;
}