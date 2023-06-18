using System.Net;
using Grpc.Core;
using Grpc.Core.Interceptors;
using JetSetGo.ReservationManagement.Application.Exceptions;

namespace JetSetGo.ReservationManagement.Grpc.Interceptors;

public class ExceptionInterceptor: Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (BadRequest ex)
        {
            var rpcException = new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            throw rpcException;
        }
        catch (NotFound ex)
        {
            var rpcException = new RpcException(new Status(StatusCode.NotFound, ex.Message));
            throw rpcException;
        }
    }
}