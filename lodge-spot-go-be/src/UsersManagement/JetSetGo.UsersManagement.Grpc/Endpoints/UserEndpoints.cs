using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Grpc.Common.Logger;
using JetSetGo.UsersManagement.Grpc.Common.Utility;
using JetSetGo.UsersManagement.Grpc.Dto;
using JetSetGo.UsersManagement.Grpc.Dto.Request;
using JetSetGo.UsersManagement.Grpc.Dto.Response;
using JetSetGo.UsersManagement.Grpc.Keycloak;
using JetSetGo.UsersManagement.Grpc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JetSetGo.UsersManagement.Grpc.Endpoints;

public static class UserEndpoints
{

    public static void MapUserEndpoints(this WebApplication application)
    {
        application.MapDelete("api/v1/users", DeleteUser);
        application.MapPost("api/v1/users/gradeHost", GradeHost);
        application.MapDelete("api/v1/users/deleteGrade", DeleteGrade);
        application.MapPut("api/v1/users/updateGrade", UpdateGrade);
        application.MapPost("api/v1/users/getGradesByHost", GetGradesByHost);
        application.MapPost("api/v1/users/getGradesByGuest", GetGradesByGuest);
        application.MapGet("api/v1/users/host/{id:Guid}", GetOutstandingHost);
        application.MapGet("api/v1/users/getUser/{id:Guid}", GetUser);
        application.MapPost("api/v1/users/tickets", BuyTickets);
    }

    private static async Task<IResult> BuyTickets(CreateTicketRequest request,[FromServices] JetSetGoService service)
    {
       var response  = await service.BuyTickets(request);
       return Results.Ok(response);
    }

    private static async Task<IResult> GetOutstandingHost([FromRoute] Guid id,
        [FromServices]HostService service)
    {
        var result = await service.GetOutstandingHost(id);
        var response = new IsOutStandingResponse { IsOutstanding = result };
        return Results.Ok(response);
        
    }

    private static async Task<IResult> DeleteGrade([FromBody]DeleteHostGradeRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var response = await gradesGrpcService.DeleteHostGrade(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> GetUser([FromRoute]Guid id, [FromServices] MyUserGrpcService myUserGrpcService)
    {
        var response = await myUserGrpcService.GetUser(id);
        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateGrade([FromBody]UpdateHostGradeRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var response = await gradesGrpcService.UpdateHostGrade(request);
        return Results.Ok(response);
    }
    
    private static async Task<IResult> GetGradesByHost([FromBody]GetGradesByHostRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var response = await gradesGrpcService.GetGradesByHost(request);
        return Results.Ok(response);
    }
    
    private static async Task<IResult> GetGradesByGuest(GetGradesByGuestRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var response = await gradesGrpcService.GetGradesByGuest(request);
        return Results.Ok(response);
    }
    private static async Task<IResult> GradeHost(HostGradeRequest request,[FromServices] GradesGrpcService gradesGrpcService)
    {
        HostGradeResponse response = await gradesGrpcService.CreateGradeForHost(request);
    
            return Results.Ok(response);
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