using System.Diagnostics;
using AutoMapper;
using Grpc.Core;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchAccommodationClient;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchReservationClient;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Services;

public class SearchAndFilterService : SearchAndFilterApp.SearchAndFilterAppBase
{
    private readonly ILogger<SearchAndFilterService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchAccommodationClient _searchAccommodationClient;
    private readonly ISearchReservationClient _searchReservationClient;
    public const string ServiceName = "SearchAndFilterService";
    public static readonly ActivitySource ActivitySource = new("Search and filter activity");

    public SearchAndFilterService(ILogger<SearchAndFilterService> logger, IMapper mapper, ISearchAccommodationClient searchAccommodationClient, ISearchReservationClient searchReservationClient)
    {
        _logger = logger;
        _mapper = mapper;
        _searchAccommodationClient = searchAccommodationClient;
        _searchReservationClient = searchReservationClient;
    }

    public override Task<GetAccommodationListResponse> SearchAndFilterAccommodations(SearchAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        _logger.LogInformation(@"Request {}",request);
        var searchRequest = _mapper.Map<SearchRequest>(request);
        var searchReservationRequest = _mapper.Map<ReservationSearchRequest>(request);
        _logger.LogInformation(@"Request {}",searchReservationRequest);
        var accommodationResponse = _searchAccommodationClient.SearchAccommodation(searchRequest);
        var reservationResponse = _searchReservationClient.SearchReservations(searchReservationRequest);
        FilterAccommodations(accommodationResponse, reservationResponse);
        activity?.Stop();
        return Task.FromResult(accommodationResponse);
    }

    private void FilterAccommodations(GetAccommodationListResponse accommodationResponse
        , GetReservationListResponse reservationListResponse)
    {
        foreach (var reservation in reservationListResponse.Reservations)
        {
           var accommodation =  accommodationResponse
               .Accommodations
               .FirstOrDefault(x => x.Id == reservation
                   .AccommodationId);
           if(accommodation is not null )
            accommodationResponse.Accommodations.Remove(accommodation);
        }
    }
}