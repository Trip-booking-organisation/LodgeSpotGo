using System.Diagnostics;
using JetSetGo.UsersManagement.Api.Common.Logger;
using JetSetGo.UsersManagement.Api.Keycloak;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JetSetGo.UsersManagement.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication application)
    {
        application.MapDelete("api/v1/users", DeleteUser);
    }

    private static async Task<IResult> DeleteUser(Guid id,IOptions<KeycloakAdmin> adminOptions,[FromServices]MessageLogger logger)
    {
        var options = adminOptions.Value;

        using var client = new HttpClient();
        var tokenUrl = $"{options.BaseUrl}/realms/master/protocol/openid-connect/token";
        logger.LogInfo("TokenUrl",tokenUrl);
        var body = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", options.AdminClientId),
            new KeyValuePair<string, string>("username", options.AdminUsername),
            new KeyValuePair<string, string>("password", options.AdminPassword)
        });
        var tokenResponse = await client.PostAsync(tokenUrl, body);
        var deletedSuccess = true;
        if (tokenResponse.IsSuccessStatusCode)
        {
            var content = await tokenResponse.Content.ReadAsStringAsync();
            logger.LogInfo("Response content",content);
            var tokenJson = JsonConvert.DeserializeObject<KeycloakTokenResponse>(content);
            var accessToken = tokenJson!.AccessToken;
            logger.LogInfo("Response tokenJson",accessToken);
            deletedSuccess = await DeleteWithToken(accessToken, client, options.Realm, id);
        }

        if (deletedSuccess) return Results.Ok();
        logger.LogError("Response tokenJson","Error");
        return Results.BadRequest();
    }

    // private static async string GetToken()
    // {
    //     
    // }

    private static async Task<bool> DeleteWithToken(string token,HttpClient client,string realm,Guid userId)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var response = await client.DeleteAsync($"http://localhost:8080/admin/realms/{realm}/users/{userId}");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("User deleted successfully.");
            return true;
        }
        Console.WriteLine($"Failed to delete user. Status code: {response.StatusCode}");
        return false;
        
    }
}