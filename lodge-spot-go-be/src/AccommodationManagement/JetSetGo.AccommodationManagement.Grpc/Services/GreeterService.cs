using Grpc.Core;
using JetSetGo.AccommodationManagement.Grpc;
using Microsoft.AspNetCore.Authorization;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }
    [Authorize(Roles = "guest")]
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}