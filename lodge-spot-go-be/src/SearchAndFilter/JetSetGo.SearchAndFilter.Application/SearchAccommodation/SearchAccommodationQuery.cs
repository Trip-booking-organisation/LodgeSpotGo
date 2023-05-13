using MediatR;

namespace JetSetGo.SearchAndFilter.Application.SearchAccommodation;

public record SearchAccommodationQuery( int NumberOfGuests,
    DateOnly StartDate,
    DateOnly EndDate,
    string Country,
    string City) : IRequest<SearchAccommodationResponse>;
