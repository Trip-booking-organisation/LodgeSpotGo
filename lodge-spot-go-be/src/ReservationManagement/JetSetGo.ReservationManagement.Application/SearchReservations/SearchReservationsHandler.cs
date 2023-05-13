using AutoMapper;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using MediatR;

namespace JetSetGo.ReservationManagement.Application.SearchReservations;

public class SearchReservationsHandler : IRequestHandler<SearchReservationsQuery, List<SearchReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public SearchReservationsHandler(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<List<SearchReservationResponse>> Handle(SearchReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _reservationRepository.SearchReservations(request);
        var result = _mapper.Map<List<SearchReservationResponse>>(reservations);
        return result;

    }
}