using System.Diagnostics;
using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Application.MessageBroker;
using JetSetGo.UsersManagement.Domain.Host;
using JetSetGo.UsersManagement.Grpc.Common.Logger;
using JetSetGo.UsersManagement.Grpc.Common.Utility;
using JetSetGo.UsersManagement.Grpc.Dto;
using JetSetGo.UsersManagement.Grpc.Dto.Request;
using JetSetGo.UsersManagement.Grpc.Dto.Response;
using JetSetGo.UsersManagement.Grpc.Keycloak;
using JetSetGo.UsersManagement.Grpc.Services;
using LodgeSpotGo.Shared.Events.Host;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JetSetGo.UsersManagement.Grpc.Endpoints;

public static class UserEndpoints
{
    public const string UserService = "UserService";
    public static  ActivitySource GetUserActivity = new("UserActivity");
    /*public const string BuyTicketsService = "BuyTickets";
    public static  ActivitySource BuyTicketsActivity = new(BuyTicketsService);
    public const string DeleteGradeService = "DeleteGrade";
    public static  ActivitySource DeleteGradeActivity = new(DeleteGradeService);
    public const string GetOutstandingHostService = "GetOutstandingHost";
    public static  ActivitySource GetOutstandingHostActivity = new ActivitySource(GetOutstandingHostService);
    public const string UpdateGradeService = "UpdateGrade";
    public static  ActivitySource UpdateGradeActivity = new(UpdateGradeService);
    public const string GetGradesByHostService = "GetGradesByHost";
    public static  ActivitySource GetGradesByHostServiceActivity = new(GetGradesByHostService);*/

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
       var activity = GetUserActivity.StartActivity(); 
       activity?.SetTag("Token", request.Token);
       var response  = await service.BuyTickets(request);
       activity?.Stop();
       return Results.Ok(response);
    }

    private static async Task<IResult> GetOutstandingHost(
        [FromRoute] Guid id,
        [FromServices]HostService service, 
        [FromServices] IOutstandingHostRepository repo,
        [FromServices] IEventBus eventBus)
    {
        var activity = GetUserActivity.StartActivity(); 
        activity?.SetTag("Id",id);
        var result = await service.GetOutstandingHost(id);
        var host = repo.GetByHostId(id);
        var isStatusChanged = await DeterminateOutstandingHostStatus(id, repo, host, result);
        if (isStatusChanged)
        {
            var @event = new OutstandingHostStatusChanged
            {
                HostId = id,
                IsOutstanding = result,
                CreatedAt = DateTime.Now
            };
            await eventBus.PublishAsync(@event);
        }
        var response = new IsOutStandingResponse { IsOutstanding = result };
        activity?.Stop();
        return Results.Ok(response);
        
    }

    private static async Task<bool> DeterminateOutstandingHostStatus(Guid id, IOutstandingHostRepository repo, OutStandingHost? host,
        bool result)
    {
        if (host is null)
        {
            var newHost = new OutStandingHost
            {
                HostId = id,
                IsOutStandingHost = result
            };
            await repo.CreateOutStandingHost(newHost);
            return false;
        }

        if (host.IsOutStandingHost == result) return false;
        host.IsOutStandingHost = result;
        await repo.Update(host);
        return true;
    }

    private static async Task<IResult> DeleteGrade([FromBody]DeleteHostGradeRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var activity = GetUserActivity.StartActivity(); 
        activity?.SetTag("GradeId",request.gradeId);
        var response = await gradesGrpcService.DeleteHostGrade(request);
        activity?.Stop();
        return Results.Ok(response);
    }

    public static async Task<IResult> GetUser([FromRoute]Guid id,[FromServices] MyUserGrpcService myUserGrpcService)
    {
        await Task.Delay(500);
        var activityListener = new ActivityListener
        {
            ShouldListenTo = s => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) =>
                ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
        };
        ActivitySource.AddActivityListener(activityListener);
        using var activity =  GetUserActivity.StartActivity();
        activity?.SetTag("Id", id.ToString());
        var response = await myUserGrpcService.GetUser(id);
        activity!.Stop();
        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateGrade([FromBody]UpdateHostGradeRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var activity = GetUserActivity.StartActivity();
        activity?.SetTag("Id", request.Id.ToString());
        var response = await gradesGrpcService.UpdateHostGrade(request);
        activity?.Stop();
        return Results.Ok(response);
    }
    
    private static async Task<IResult> GetGradesByHost([FromBody]GetGradesByHostRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var activity = GetUserActivity.StartActivity();
        activity?.SetTag("HostId", request.HostId.ToString());
        var response = await gradesGrpcService.GetGradesByHost(request);
        activity?.Stop();
        return Results.Ok(response);
    }
    
    private static async Task<IResult> GetGradesByGuest(GetGradesByGuestRequest request, [FromServices]GradesGrpcService gradesGrpcService)
    {
        var response = await gradesGrpcService.GetGradesByGuest(request);
        return Results.Ok(response);
    }
    private static IResult GradeHost(HostGradeRequest request,[FromServices] GradesGrpcService gradesGrpcService)
    {
        var response = gradesGrpcService.CreateGradeForHost(request);
        if (response.IsFailed)
        {
            Results.BadRequest(response.Errors.ToString());
        }

        return Results.Ok(response.Value);
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