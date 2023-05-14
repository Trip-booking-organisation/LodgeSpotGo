using AutoMapper;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodation;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;

public class MappingToGrpcResponse : IMappingToGrpcResponse
{
    private readonly IMapper _mapper;

    public MappingToGrpcResponse(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<GetAccommodationListResponse> MapSearchToGrpcResponse(List<SearchAccommodationResponse> list)
    {
        var response = new GetAccommodationListResponse();
        var responseList = list.Select(accommodation => _mapper.Map<AccommodationDto>(accommodation)).ToList();
        responseList.ForEach(dto => response.Accommodations.Add(dto));
        return Task.FromResult(response);
    }

    public Task<GetAccommodationResponse> MapAccommodationToGrpcResponse(Accommodation accommodation)
    {
        var response = new GetAccommodationResponse
        {
            Accommodation = _mapper.Map<AccommodationDto>(accommodation)
        };
        return Task.FromResult(response);
    }

    public Task<GetAccommodationsByHostResponse> MapAccommodationsToGrpcResponse(List<Accommodation> list)
    {
        var response = new GetAccommodationsByHostResponse();
        var responseList = list.Select(accommodation => _mapper.Map<AccommodationDto>(accommodation)).ToList();
        responseList.ForEach(dto => response.Accommodations.Add(dto));
        return Task.FromResult(response);
    }
}