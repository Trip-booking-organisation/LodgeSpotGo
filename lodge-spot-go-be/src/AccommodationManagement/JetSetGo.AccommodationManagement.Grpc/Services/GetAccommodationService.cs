﻿using System.Diagnostics;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class GetAccommodationService : GetAccommodationApp.GetAccommodationAppBase
{
    private readonly ILogger<GetAccommodationService> _logger;
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IMappingToGrpcResponse _mappingToGrpcResponse;
    public static readonly ActivitySource ActivitySource = new("GetAccommodationActivity");

    public GetAccommodationService(ILogger<GetAccommodationService> logger, IAccommodationRepository accommodationRepository, IMappingToGrpcResponse mappingToGrpcResponse)
    {
        _logger = logger;
        _accommodationRepository = accommodationRepository;
        _mappingToGrpcResponse = mappingToGrpcResponse;
    }

    public override async Task<GetAccommodationResponse> GetAccommodation(GetAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("AccommodationID", request.Id);
        _logger.LogInformation(@"REQUEST CAME IN");
        _logger.LogInformation(@"Request {}",request.Id);
        var accommodation = await _accommodationRepository.GetAsync(Guid.Parse(request.Id));
        var response = _mappingToGrpcResponse.MapAccommodationToGrpcResponse(accommodation);
        _logger.LogInformation(@"Request {}",response.Result.Accommodation.ToString());
        activity?.Stop();
        return await response;
    }
}