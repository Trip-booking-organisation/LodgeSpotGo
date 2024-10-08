﻿using System.Diagnostics;
using AutoMapper;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Application.MessageBroker;
using JetSetGo.AccommodationManagement.Domain.Accommodations;
using JetSetGo.AccommodationManagement.Domain.Accommodations.ValueObjects;
using JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;
using LodgeSpotGo.Shared.Events.Accommdation;
using Microsoft.AspNetCore.Authorization;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class AccommodationService : AccommodationApp.AccommodationAppBase
{
    private readonly ILogger<AccommodationService> _logger;
    private readonly IAccommodationRepository _repository;
    private readonly IMappingToGrpcResponse _mappingToGrpcResponse;
    private readonly IMapper _mapper;
    private readonly IEventBus _bus;
    public const string ServiceName = "AccommodationService";
    public static readonly ActivitySource ActivitySource = new("Accommodation activity");


    public AccommodationService(
        ILogger<AccommodationService> logger, 
        IAccommodationRepository repository, 
        IMapper mapper, 
        IMappingToGrpcResponse mappingToGrpcResponse, 
        IEventBus bus)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _mappingToGrpcResponse = mappingToGrpcResponse;
        _bus = bus;
    }
    /*[Authorize(Roles = "guest,host")]*/
    public override async  Task<GetAccommodationListResponse> GetAccommodationList(GetAccommodationListRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var list = new GetAccommodationListResponse();
        var accommodations = await _repository.GetAllAsync();
        _logger.LogInformation(@"List {}",accommodations.Select(accommodation => accommodation.Id.ToString()));
        var responseList = accommodations.Select(accommodation => _mapper.Map<AccommodationDto>(accommodation)).ToList();
        
        responseList.ForEach(dto => list.Accommodations.Add(dto));
        activity?.SetTag("AccommodationList", responseList);
        activity?.Stop();
        return list;
    }
    [Authorize(Roles = "host")]
    public override Task<CreateAccommodationResponse> CreateAccommodation(CreateAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("Accommodation name", request.Accommodation.Name);
        _logger.LogInformation(@"Request {request.Accommodation}",request.Accommodation);
        var accommodation = new Accommodation
        {
            Name = request.Accommodation.Name,
            Address = new Address
            {
                City = request.Accommodation.Address.City,
                Country = request.Accommodation.Address.Country,
                Street = request.Accommodation.Address.Street
            },
            Amenities = request.Accommodation.Amenities.ToList(),
            MaxGuests = request.Accommodation.MaxGuests,
            MinGuests = request.Accommodation.MinGuests,
            Photos = request.Accommodation.Photos
                .Select(x => new AccommodationPhoto
            {
                Photo = x
            }).ToList(),
            SpecalPrices = request.Accommodation.SpecialPrices
                .Select(a => new SpecialPrice
                {
                    Price = a.Price,
                    DateRange = new DateRange
                    {
                        From = a.DateRange.From.ToDateTime(),
                        To = a.DateRange.To.ToDateTime()
                    }
                }).ToList(),
            HostId = Guid.Parse(request.Accommodation.HostId),
            AutomaticConfirmation = request.Accommodation.AutomaticConfirmation
            
        };
        _repository.CreateAsync(accommodation);
        var @event = new AccommodationCreatedEvent
        {
            Name = accommodation.Name,
            Id = accommodation.Id.ToString()
        };
        _bus.PublishAsync(@event);
        activity?.Stop();
        return Task.FromResult(new CreateAccommodationResponse
        {
            Location = "api/v1/accommodations/{id}"
        });
    }
    /*[Authorize(Roles = "host")]*/
    public override async Task<UpdateAccommodationResponse> UpdateAccommodationPrice(UpdateAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("AccommodationId", request.Accommodation.AccommodationId);
        var accId = new Guid(request.Accommodation.AccommodationId);
        var accommodation = await _repository.GetAsync(accId);
        accommodation.SpecalPrices.Add(new SpecialPrice
        {
            DateRange = new DateRange
            {
                From = request.Accommodation.Price.DateRange.From.ToDateTime(),
                To = request.Accommodation.Price.DateRange.To.ToDateTime()
            },
            Price = request.Accommodation.Price.Price
        });
        await _repository.UpdateAsync(accommodation);
        activity?.Stop();
        return new UpdateAccommodationResponse
        {
            IsSuccess = true
        };
    }
    /*[Authorize(Roles = "host")]*/
    public override async Task<GetAccommodationResponse> GetAccommodation(GetAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("AccommodationId", request.Id);
        _logger.LogInformation(@"Request {}",request.Id);
        var accommodation = await _repository.GetAsync(Guid.Parse(request.Id));
        var response = _mappingToGrpcResponse.MapAccommodationToGrpcResponse(accommodation);
        activity?.Stop();
        return await response;
    }
    /*[Authorize(Roles = "host")]*/
    public override async Task<GetAccommodationsByHostResponse> GetAccommodationByHost(GetAccommodationsByHostRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("HostId", request.HostId);
        var accommodations = await _repository.GetByHost(Guid.Parse(request.HostId));
        var response = _mappingToGrpcResponse.MapAccommodationsToGrpcResponse(accommodations);
        activity?.Stop();
        return await response;
    }
}