using JetSetGo.UsersManagement.Grpc.Keycloak;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JetSetGo.UsersManagement.Grpc.Common.Utility;

public class KeyCloakConnections
{
    private readonly KeycloakAdmin _keycloakAdmin;
    private readonly TokenService _tokenService;
    private readonly HttpClient _httpClient;

    public KeyCloakConnections(IOptions<KeycloakAdmin> adminOptions, TokenService tokenService, HttpClient httpClient)
    {
        _tokenService = tokenService;
        _httpClient = httpClient;
        _keycloakAdmin = adminOptions.Value;
    }

    public async Task<KeycloakUserResponse?> GetUserIdAsync(Guid userId)
    {
        var accessToken = await _tokenService.GenerateToken();
        Console.WriteLine($"UserId:  {userId}");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Value}");
        var getLink = $"http://localhost:8080/admin/realms/{_keycloakAdmin.Realm}/users/{userId}";
        Console.WriteLine($"getLink:  {getLink}");
        var request = new HttpRequestMessage(HttpMethod.Get, getLink);
       
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<KeycloakUserResponse>(responseBody);
        return result;
    }
}