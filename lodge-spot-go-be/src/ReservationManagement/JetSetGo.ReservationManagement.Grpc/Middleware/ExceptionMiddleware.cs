using System.Net;
using JetSetGo.ReservationManagement.Application.Exceptions;
using JetSetGo.ReservationManagement.Grpc.Exception;
using Newtonsoft.Json;

namespace JetSetGo.ReservationManagement.Grpc.Middleware;

public class ExceptionMiddleware : IMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception e)
        {
            switch (e)
            {
                case BadRequest:
                    await BadRequestHandler(context, e);
                    break;
                default:
                    await UnknownExceptionHandler(context, e);
                    break;
            }
        }
    }
    private static async Task BadRequestHandler(HttpContext context, System.Exception e)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        var responseContent = new ErrorObject
        {
            Error = e.Message
        };
        var jsonResult = JsonConvert.SerializeObject(responseContent);
        await context.Response.WriteAsync(jsonResult);
    }
    private static async Task UnknownExceptionHandler(HttpContext context, System.Exception e)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.Conflict;
        var responseContent = new ErrorObject()
        {
            Error = e.Message,
        };
        var jsonResult = JsonConvert.SerializeObject(responseContent);
        await context.Response.WriteAsync(jsonResult);
    }
}