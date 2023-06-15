using Grpc.Core;
using LodgeSpotGo.UserManagement.Grpc;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class FilterService :FilterOutstandingHostApp.FilterOutstandingHostAppBase
{
    private readonly ILogger<FilterService> _logger;
    private readonly HostService _hostService;

    public FilterService(ILogger<FilterService> logger, HostService hostService)
    {
        _logger = logger;
        _hostService = hostService;
    }

    public override async Task<FiletOutstandingHostResponse> IsOutstanding(FilterOutstandingHostRequest request, ServerCallContext context)
    {
        _logger.LogInformation("-----------------Request came in");
        var isHostOutstanding =  await _hostService.GetOutstandingHost(Guid.Parse(request.HostId));
        return new FiletOutstandingHostResponse
        {
            IsOutstanding = isHostOutstanding
        };
    }
}