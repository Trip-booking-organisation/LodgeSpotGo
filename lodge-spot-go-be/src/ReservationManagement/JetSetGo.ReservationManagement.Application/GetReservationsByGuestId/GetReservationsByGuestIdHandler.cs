using AutoMapper;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using MediatR;

namespace JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;

public class GetReservationsByGuestIdHandler : IRequestHandler<GetReservationsByGuestIdQuery,List<GetReservationsByGuestIdCommandResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public GetReservationsByGuestIdHandler(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<List<GetReservationsByGuestIdCommandResponse>> Handle(GetReservationsByGuestIdQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _reservationRepository.GetByGuestId(request.GuestId);
        var responseList = reservations.Select(reservation => _mapper.Map<GetReservationsByGuestIdCommandResponse>(reservation)).ToList();
        return responseList;
    }
}