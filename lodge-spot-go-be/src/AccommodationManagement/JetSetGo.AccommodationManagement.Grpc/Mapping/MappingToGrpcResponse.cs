using AutoMapper;
using JetSetGo.AccommodationManagement.Application.SearchAccommodation;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping;

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
}