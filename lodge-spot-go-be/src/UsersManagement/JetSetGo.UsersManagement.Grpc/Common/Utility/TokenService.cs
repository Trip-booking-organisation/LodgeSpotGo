using FluentResults;
using JetSetGo.UsersManagement.Grpc.Keycloak;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JetSetGo.UsersManagement.Grpc.Common.Utility;

public class TokenService
{
    private readonly KeycloakAdmin _adminOptions;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IOptions<KeycloakAdmin> adminOptions, ILogger<TokenService> logger)
    {
        _logger = logger;
        _adminOptions = adminOptions.Value;
    }


    public async Task<Result<string>> GenerateToken()
    {
        using var client = new HttpClient();
        var tokenUrl = $"{_adminOptions.BaseUrl}/realms/master/protocol/openid-connect/token";
        _logger.LogInformation("TokenUrl {}",tokenUrl);
        var body = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _adminOptions.AdminClientId),
            new KeyValuePair<string, string>("username", _adminOptions.AdminUsername),
            new KeyValuePair<string, string>("password", _adminOptions.AdminPassword)
        });
        var tokenResponse = await client.PostAsync(tokenUrl, body);
        if (!tokenResponse.IsSuccessStatusCode) return Result.Fail("Cannot generate token!");
        var content = await tokenResponse.Content.ReadAsStringAsync();
        _logger.LogInformation("Response content {}",content);
        var tokenJson = JsonConvert.DeserializeObject<KeycloakTokenResponse>(content);
        var accessToken = tokenJson!.AccessToken;
        _logger.LogInformation("Response tokenJson {}",accessToken);
        return accessToken;

    }
}