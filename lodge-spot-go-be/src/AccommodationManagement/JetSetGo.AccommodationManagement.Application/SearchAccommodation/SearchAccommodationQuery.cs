using MediatR;

namespace JetSetGo.AccommodationManagement.Application.SearchAccommodation;

public record SearchAccommodationQuery( int NumberOfGuests,
    string Country,
    string City) : IRequest<List<SearchAccommodationResponse>>;
