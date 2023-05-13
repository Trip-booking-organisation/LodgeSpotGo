using MediatR;

namespace JetSetGo.ReservationManagement.Application.UpdateReservationStatus;

public record UpdateReservationStatusCommand(Guid Id,
    string Status) : IRequest<UpdateReservationStatusCommandResponse>;