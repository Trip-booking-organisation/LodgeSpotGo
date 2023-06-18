using Grpc.Core;
using Grpc.Core.Interceptors;

namespace JetSetGo.ReservationManagement.Grpc.Interceptors;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger _logger;

    public LoggingInterceptor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<LoggingInterceptor>();
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogInformation($"Starting call. Type: {context.Method.Type}. " +
                               $"Method: {context.Method.Name}.");
        return continuation(request, context);
    }
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation($"Received request. Method: {context.Method}, Request: {request}");

        return continuation(request, context);
    }

}