using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodation;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;

public interface IMappingToGrpcResponse
{
    Task<GetAccommodationListResponse> MapSearchToGrpcResponse(List<SearchAccommodationResponse> list);
    Task<GetAccommodationResponse> MapAccommodationToGrpcResponse(Accommodation list);
}