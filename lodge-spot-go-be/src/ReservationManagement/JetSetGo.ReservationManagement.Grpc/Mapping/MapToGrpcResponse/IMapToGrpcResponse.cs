using JetSetGo.ReservationManagement.Application.SearchReservations;

namespace JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;

public interface IMapToGrpcResponse
{
    Task<GetReservationListResponse> MapSearchToGrpcResponse(List<SearchReservationResponse> list);

}