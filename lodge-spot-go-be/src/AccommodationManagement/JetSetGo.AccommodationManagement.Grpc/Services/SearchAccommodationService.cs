using AutoMapper;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Grpc.Mapping;
using LodgeSpotGo.SearchAndFilter.Grpc;
using MediatR;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class SearchAccommodationService : SearchAccommodationApp.SearchAccommodationAppBase
{
    private readonly ILogger<SearchAccommodationService> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly IMappingToGrpcResponse _mappingToGrpcResponse;

    public SearchAccommodationService(ILogger<SearchAccommodationService> logger, ISender sender, IMapper mapper, IMappingToGrpcResponse mappingToGrpcResponse)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
        _mappingToGrpcResponse = mappingToGrpcResponse;
    }

    public override Task<GetAccommodationListResponse> SearchAccommodations(SearchRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"-------------------Accommodation service, request came in {}", request.ToString());
        var query = _mapper.Map<SearchAccommodationQuery>(request);
        var response = _sender.Send(query);
        _logger.LogInformation(@"------Response from db : {}", response.Result.Count);
        var result = _mappingToGrpcResponse.MapSearchToGrpcResponse(response.Result);
        _logger.LogInformation(@"------Response from db : {}", result);
        return result;
    }
}