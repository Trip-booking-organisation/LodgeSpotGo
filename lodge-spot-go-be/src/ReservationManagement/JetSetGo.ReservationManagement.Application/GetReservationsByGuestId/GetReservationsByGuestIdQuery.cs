using MediatR;

namespace JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;

public record GetReservationsByGuestIdQuery(Guid GuestId) : IRequest<List<GetReservationsByGuestIdCommandResponse>>;