using MediatR;

namespace JetSetGo.ReservationManagement.Application.CancelReservation;

public record CancelReservationCommand(Guid Id) : IRequest<CancelReservationCommandResponse>;