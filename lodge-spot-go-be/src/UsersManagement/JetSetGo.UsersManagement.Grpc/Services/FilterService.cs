using System.Diagnostics;
using Grpc.Core;
using LodgeSpotGo.UserManagement.Grpc;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class FilterService :FilterOutstandingHostApp.FilterOutstandingHostAppBase
{
    private readonly ILogger<FilterService> _logger;
    private readonly HostService _hostService;
    public static ActivitySource Activity = new("FilterUserActivity");

    public FilterService(ILogger<FilterService> logger, HostService hostService)
    {
        _logger = logger;
        _hostService = hostService;
    }

    public override async Task<FiletOutstandingHostResponse> IsOutstanding(FilterOutstandingHostRequest request, ServerCallContext context)
    {
        var activity = Activity.StartActivity();
        activity?.SetTag("HostId", request.HostId);
        _logger.LogInformation("-----------------Request came in");
        var isHostOutstanding =  await _hostService.GetOutstandingHost(Guid.Parse(request.HostId));
        activity?.Stop();
        return new FiletOutstandingHostResponse
        {
            IsOutstanding = isHostOutstanding
        };
    }
}