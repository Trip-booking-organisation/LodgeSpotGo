using AutoMapper;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using MediatR;

namespace JetSetGo.AccommodationManagement.Application.SearchAccommodation;

public class SearchAccommodationHandler : IRequestHandler<SearchAccommodationQuery,List<SearchAccommodationResponse>>
{
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IMapper _mapper;

    public SearchAccommodationHandler(IAccommodationRepository accommodationRepository, IMapper mapper)
    {
        _accommodationRepository = accommodationRepository;
        _mapper = mapper;
    }

    public async Task<List<SearchAccommodationResponse>> Handle(SearchAccommodationQuery request, CancellationToken cancellationToken)
    {
        var accommodations = await _accommodationRepository.SearchAccommodations(request);
        var result = _mapper.Map<List<SearchAccommodationResponse>>(accommodations);
        return result;
    }
}