namespace JetSetGo.ReservationManagement.Application.Exceptions;

public class BadRequest : Exception
{
    public BadRequest(string? message) : base(message)
    {
    }
}