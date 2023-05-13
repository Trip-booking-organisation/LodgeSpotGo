using MediatR;

namespace JetSetGo.SearchAndFilter.Application.SearchAccommodation;

public class SearchAccommodationHandler : IRequestHandler<SearchAccommodationQuery,SearchAccommodationResponse>
{
    public Task<SearchAccommodationResponse> Handle(SearchAccommodationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}