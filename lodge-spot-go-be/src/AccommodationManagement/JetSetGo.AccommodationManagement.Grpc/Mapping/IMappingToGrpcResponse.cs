using JetSetGo.AccommodationManagement.Application.SearchAccommodation;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping;

public interface IMappingToGrpcResponse
{
    Task<GetAccommodationListResponse> MapSearchToGrpcResponse(List<SearchAccommodationResponse> list);
}