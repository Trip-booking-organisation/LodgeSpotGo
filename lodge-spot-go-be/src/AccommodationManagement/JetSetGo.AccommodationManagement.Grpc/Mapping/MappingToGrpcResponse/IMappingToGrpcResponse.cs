using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodations;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping.MappingToGrpcResponse;

public interface IMappingToGrpcResponse
{
    Task<GetAccommodationListResponse> MapSearchToGrpcResponse(List<SearchAccommodationResponse> list);
    Task<GetAccommodationResponse> MapAccommodationToGrpcResponse(Accommodation list);
    Task<GetAccommodationsByHostResponse> MapAccommodationsToGrpcResponse(List<Accommodation> list);
}