﻿using System.Diagnostics;
using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Grpc.Common.Logger;
using JetSetGo.UsersManagement.Grpc.Common.Utility;
using JetSetGo.UsersManagement.Grpc.Keycloak;
using JetSetGo.UsersManagement.Grpc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JetSetGo.UsersManagement.Grpc.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication application)
    {
        application.MapDelete("api/v1/users", DeleteUser);
    }

    private static async Task<IResult> DeleteUser(Guid userId,string role,
        IOptions<KeycloakAdmin> adminOptions,
        [FromServices]MessageLogger logger,
        [FromServices]MyUserGrpcService myUserGrpcService,
        [FromServices] TokenService tokenService,
        [FromServices] MyUserGrpcService grpcService)
    {
        var response = grpcService.GetReservationResponse(new UserRequest
        {
            UserId = userId.ToString(),
            Role = role
        });
        logger.LogInfo(response.HasReservation.ToString(),"Mess");
        if (response.HasReservation)
        {
            return Results.Conflict("User have reservations!");
        }
        
        var tokenResult = await tokenService.GenerateToken();
        if (tokenResult.IsFailed)
        {
           return Results.Conflict("Cannot generate token");
        }

        var tokenResponse = tokenResult.Value!;
        var deletedSuccess = await DeleteWithToken(tokenResponse, adminOptions.Value.Realm, userId);
        if (deletedSuccess) return Results.NoContent();
        logger.LogError("Response tokenJson","Error");
        return Results.BadRequest();
    }

    private static async Task<bool> DeleteWithToken(string token,string realm,Guid userId)
    {
        using var client = new HttpClient();
        Console.WriteLine($"UserId:  {userId}");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var postLink = $"http://localhost:8080/admin/realms/{realm}/users/{userId}";
        Console.WriteLine($"postLink:  {postLink}");
        var response = await client.DeleteAsync(postLink);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("User deleted successfully.");
            return true;
        }
        Console.WriteLine($"Failed to delete user. Status code: {response.StatusCode}");
        return false;
        
    }
}