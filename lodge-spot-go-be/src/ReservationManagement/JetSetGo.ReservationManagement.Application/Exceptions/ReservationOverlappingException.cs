namespace JetSetGo.ReservationManagement.Application.Exceptions;

public class ReservationOverlappingException : BadRequest
{
    public ReservationOverlappingException(string? message) : base(message)
    {
    }
}