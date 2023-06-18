using Grpc.Core;
using JetSetGo.UsersManagement.Grpc.Common.Utility;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class GetUserInfoService: GetUserKeycloakService.GetUserKeycloakServiceBase
{
    private readonly ILogger<GetUserInfoService> _logger;
    private readonly KeyCloakConnections _connections;
    public GetUserInfoService(ILogger<GetUserInfoService> logger, KeyCloakConnections connections)
    {
        _logger = logger;
        _connections = connections;
    }
    public override async Task<GetUserResponseKeycloak> GetUserInfo(GetUserRequestKeycloak request, ServerCallContext context)
    {
        try
        {
            var user = await _connections.GetUserIdAsync(Guid.Parse(request.Id));
            _logger.LogInformation(@"----------------------Request came in -------------{}", request.Id);
            var result = new GetUserResponseKeycloak {
                LastName = user!.LastName,
                Id = user.Id,
                Mail = user.Email,
                Name = user.FirstName
            };
            return result;
        }
        catch
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User with given id doesnt exists!"));
        }
    }
}