using AutoMapper;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Domain.Accommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodation.ValueObjects;
using JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class AccommodationService : AccommodationApp.AccommodationAppBase
{
    private readonly ILogger<AccommodationService> _logger;
    private readonly IAccommodationRepository _repository;
    private readonly IMappingToGrpcResponse _mappingToGrpcResponse;
    private readonly IMapper _mapper;

    public AccommodationService(ILogger<AccommodationService> logger, IAccommodationRepository repository, IMapper mapper, IMappingToGrpcResponse mappingToGrpcResponse)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _mappingToGrpcResponse = mappingToGrpcResponse;
    }
    /*[Authorize(Roles = "guest,host")]*/
    public override async  Task<GetAccommodationListResponse> GetAccommodationList(GetAccommodationListRequest request, ServerCallContext context)
    {
        var list = new GetAccommodationListResponse();
        var accommodations = await _repository.GetAllAsync();
        _logger.LogInformation(@"List {}",accommodations.ToString());
        var responseList = accommodations.Select(accommodation => _mapper.Map<AccommodationDto>(accommodation)).ToList();
        
        responseList.ForEach(dto => list.Accommodations.Add(dto));
        return list;
    }
    /*[Authorize(Roles = "host")]*/
    public override Task<CreateAccommodationResponse> CreateAccommodation(CreateAccommodationRequest request, ServerCallContext context)
    {
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
                Photo = x.Photo
            }).ToList(),
            SpecalPrices = request.Accommodation.SpecialPrices
                .Select(a => new SpecalPrice
                {
                    Price = a.Price,
                    DateRange = new DateRange
                    {
                        From = a.DateRange.From.ToDateTime(),
                        To = a.DateRange.To.ToDateTime()
                    }
                }).ToList(),
            HostId = Guid.Parse(request.Accommodation.HostId)
            
        };
        _repository.CreateAsync(accommodation);
        return Task.FromResult(new CreateAccommodationResponse
        {
            CreatedId = Guid.NewGuid().ToString()
        });
    }

    public override async Task<GetAccommodationResponse> GetAccommodation(GetAccommodationRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"Request {}",request.Id);
        var accommodation = await _repository.GetAsync(Guid.Parse(request.Id));
        var response = _mappingToGrpcResponse.MapAccommodationToGrpcResponse(accommodation);
        return await response;
    }

    public override async Task<GetAccommodationsByHostResponse> GetAccommodationByHost(GetAccommodationsByHostRequest request, ServerCallContext context)
    {
        var accommodations = await _repository.GetByHost(Guid.Parse(request.HostId));
        var response = _mappingToGrpcResponse.MapAccommodationsToGrpcResponse(accommodations);
        return await response;
    }
}