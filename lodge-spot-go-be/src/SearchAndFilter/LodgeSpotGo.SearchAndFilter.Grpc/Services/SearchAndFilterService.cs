using AutoMapper;
using Grpc.Core;
using JetSetGo.SearchAndFilter.Application.SearchAccommodation;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.SearchAccommodationClient;
using MediatR;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Services;

public class SearchAndFilterService : SearchAndFilterApp.SearchAndFilterAppBase
{
    private readonly ILogger<SearchAndFilterService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchAccommodationClient _searchAccommodationClient;

    public SearchAndFilterService(ILogger<SearchAndFilterService> logger, IMapper mapper, ISearchAccommodationClient searchAccommodationClient)
    {
        _logger = logger;
        _mapper = mapper;
        _searchAccommodationClient = searchAccommodationClient;
    }

    public override Task<GetAccommodationListResponse> SearchAndFilterAccommodations(SearchAccommodationRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"Request {}",request);
        var searchRequest = _mapper.Map<SearchRequest>(request);
        var response = _searchAccommodationClient.SearchAccommodation(searchRequest);
        return Task.FromResult(response);
    }
}