using MediatR;

namespace JetSetGo.ReservationManagement.Application.SearchReservations;

public record SearchReservationsQuery(DateTime StartDate,
    DateTime EndDate) : IRequest<List<SearchReservationResponse>>; 
