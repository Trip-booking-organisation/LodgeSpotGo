using System.Diagnostics;
using AutoMapper;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodations;
using JetSetGo.AccommodationManagement.Grpc.Clients.Recommendation;
using JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;
using JetSetGo.RecommodationSystem.Grpc;
using MediatR;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class SearchAccommodationService : SearchAccommodationApp.SearchAccommodationAppBase
{
    private readonly ILogger<SearchAccommodationService> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly IMappingToGrpcResponse _mappingToGrpcResponse;
    private readonly IRecommendationClient _recommendationClient;
    private readonly IAccommodationRepository _accommodationRepository;
    public const string ServiceName = "GradesService";
    public static readonly ActivitySource ActivitySource = new(ServiceName);

    public SearchAccommodationService(ILogger<SearchAccommodationService> logger, ISender sender, IMapper mapper, IMappingToGrpcResponse mappingToGrpcResponse, IRecommendationClient recommendationClient, IAccommodationRepository accommodationRepository)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
        _mappingToGrpcResponse = mappingToGrpcResponse;
        _recommendationClient = recommendationClient;
        _accommodationRepository = accommodationRepository;
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

    public override async  Task<GetAccommodationListResponse> GetRecommendedAccommodations(GetRecommodationReqest request, ServerCallContext context)
    {
        var list = new GetAccommodationListResponse();
        var recommendedAccomms = _recommendationClient.GetRecommendations(request);
        List<Accommodation> accommodations = new List<Accommodation>();
        GetAccommodationListResponse response = new GetAccommodationListResponse();
        foreach (var acc in recommendedAccomms.Response)
        {
            accommodations.Add( await _accommodationRepository.GetAsync(Guid.Parse(acc.Id)));
        }
        var responseList = accommodations.Select(accommodation => _mapper.Map<AccommodationDto>(accommodation)).ToList();
        responseList.ForEach(dto => list.Accommodations.Add(dto));
        return list;
    }
}