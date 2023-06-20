using System.Diagnostics;
using AutoMapper;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodations;
using JetSetGo.AccommodationManagement.Grpc.Clients.Recommendation;
using JetSetGo.AccommodationManagement.Grpc.Dto;
using JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;
using JetSetGo.RecommodationSystem.Grpc;
using LodgeSpotGo.AccommodationManagement.Grpc;
using MediatR;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class SearchAccommodationService : SearchAccommodationApp.SearchAccommodationAppBase
{
    private readonly ILogger<SearchAccommodationService> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly IMappingToGrpcResponse _mappingToGrpcResponse;
    public static readonly ActivitySource ActivitySource = new("SearchAccommodationActivity");
    private readonly IRecommendationClient _recommendationClient;
    private readonly IAccommodationRepository _accommodationRepository;
    public const string ServiceName = "GradesService";
    public static readonly ActivitySource ActivitySource = new(ServiceName);
    private readonly IGradeRepository _gradeRepository;

    public SearchAccommodationService(ILogger<SearchAccommodationService> logger, ISender sender, IMapper mapper, IMappingToGrpcResponse mappingToGrpcResponse, IRecommendationClient recommendationClient, IAccommodationRepository accommodationRepository, IGradeRepository gradeRepository)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
        _mappingToGrpcResponse = mappingToGrpcResponse;
        _recommendationClient = recommendationClient;
        _accommodationRepository = accommodationRepository;
        _gradeRepository = gradeRepository;
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
        List<AccommodationWithGrade> accommodations = new List<AccommodationWithGrade>();
        List<Accommodation> accommodationsSorted = new List<Accommodation>();
        GetAccommodationListResponse response = new GetAccommodationListResponse();
        accommodationsSorted = await SortByGrade(recommendedAccomms);
        
        var responseList = accommodationsSorted.Select(accommodation => _mapper.Map<AccommodationDto>(accommodation)).ToList();
        responseList.ForEach(dto => list.Accommodations.Add(dto));
        return list;
    }

    private async Task<List<Accommodation>> SortByGrade(GetRecommodationsResponse recommendedAccomms)
    {
        List<AccommodationWithGrade> accommodations = new List<AccommodationWithGrade>();
        List<Accommodation> sortedAccoms = new List<Accommodation>();
        foreach (var acc in recommendedAccomms.Response)
        {
            var req = new GetAverageGradeByAccommodationRequest { AccommodationId = acc.Id.ToString() };
            var grade = await GetAverageGrade(req);
            if (grade.AverageGradeNumber >= 0)
            {
                var accomm = await _accommodationRepository.GetAsync(Guid.Parse(acc.Id));
                var gradedAcomm = new AccommodationWithGrade
                {
                    Accommodation = accomm,
                    Grade = grade.AverageGradeNumber
                };
                accommodations.Add(gradedAcomm);
            }

        }

        accommodations = accommodations.OrderByDescending(a => a.Grade).ToList();
        return mapAccGradeToAcc(accommodations);
    }

    private List<Accommodation> mapAccGradeToAcc(List<AccommodationWithGrade> accommodations)
    {
        List<Accommodation> accoms = new List<Accommodation>();
        foreach (var acc in accommodations)
        {
            accoms.Add(acc.Accommodation);
        }

        return accoms;
    }
    
    
    public async Task<GetAverageGradeByAccommodationResponse> GetAverageGrade(GetAverageGradeByAccommodationRequest request)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("AccommodationId", request.AccommodationId);
        var grades = await _gradeRepository.GetByAccommodation(Guid.Parse(request.AccommodationId));
        if (grades.Count != 0)
        {
            var gradeNumber = 0;
            grades.ForEach(x =>
            {
                gradeNumber += x.Number;
            });
            var averageGrade = gradeNumber / grades.Count;
            activity?.Stop();
            return new GetAverageGradeByAccommodationResponse
            {
                AverageGradeNumber = averageGrade
            };
        }

        return new GetAverageGradeByAccommodationResponse
        {
            AverageGradeNumber = 3
        };

    }
    
    
}