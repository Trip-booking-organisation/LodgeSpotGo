using System.Diagnostics;
using AutoMapper;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;
using MediatR;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class SearchAccommodationService : SearchAccommodationApp.SearchAccommodationAppBase
{
    private readonly ILogger<SearchAccommodationService> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly IMappingToGrpcResponse _mappingToGrpcResponse;
    public static readonly ActivitySource ActivitySource = new("SearchAccommodationActivity");

    public SearchAccommodationService(ILogger<SearchAccommodationService> logger, ISender sender, IMapper mapper, IMappingToGrpcResponse mappingToGrpcResponse)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
        _mappingToGrpcResponse = mappingToGrpcResponse;
    }

    public override async Task<GetAccommodationListResponse> SearchAccommodations(SearchRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("NumberOfGuests", request.NumberOfGuests);
        activity?.SetTag("Country", request.Country);
        activity?.SetTag("City", request.City);
        _logger.LogInformation(@"-------------------Accommodation service, request came in {}", request.ToString());
        var query = _mapper.Map<SearchAccommodationQuery>(request);
        var response = await _sender.Send(query);
        _logger.LogInformation(@"------Response from db : {}", response.Count);
        var result = _mappingToGrpcResponse.MapSearchToGrpcResponse(response);
        _logger.LogInformation(@"------Response from db : {}", result);
        activity?.Stop();
        return await result;
    }
}