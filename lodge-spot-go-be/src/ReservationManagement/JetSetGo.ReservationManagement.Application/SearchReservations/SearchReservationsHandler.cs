using AutoMapper;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
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
        var dateRange = new DateRange
        {
            From = request.StartDate,
            To = request.EndDate
        };
        var reservations = await _reservationRepository.GetAllAsync(cancellationToken);
        var reserv = (from res in reservations 
                        let isOverlapping = res.IsOverlapping(dateRange) 
                             where isOverlapping select res).ToList();
        var result = _mapper.Map<List<SearchReservationResponse>>(reserv);
        return result;

    }
}