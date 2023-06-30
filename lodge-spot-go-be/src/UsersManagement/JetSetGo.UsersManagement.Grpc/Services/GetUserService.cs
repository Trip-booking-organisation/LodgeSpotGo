using System.Diagnostics;
using Grpc.Core;
using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Grpc.Common.Utility;
using JetSetGo.UsersManagement.Grpc.Keycloak;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class GetUserService : UserApp.UserAppBase
{
    private readonly ILogger<GetUserService> _logger;
    private readonly KeyCloakConnections _connections;
    public static ActivitySource Activity = new("GetUserActivity");
    public GetUserService(ILogger<GetUserService> logger, KeyCloakConnections connections)
    {
        _logger = logger;
        _connections = connections;
    }

    public override async Task<GetUserResponse> GetUserById(GetUserRequest request, ServerCallContext context)
    {
        var activity = Activity.StartActivity();
        activity?.SetTag("Id", request.Id);
        var user = await _connections.GetUserIdAsync(Guid.Parse(request.Id));
        
        _logger.LogInformation(@"----------------------Request came in -------------{}", request.Id);
        var result = new GetUserResponse { 
            User = new GetUserDto
            {
                LastName = user!.LastName,
                Id = user.Id,
                Mail = user.Email,
                Name = user.FirstName
            }
        };
        activity?.Stop();
        return result;
    }
}