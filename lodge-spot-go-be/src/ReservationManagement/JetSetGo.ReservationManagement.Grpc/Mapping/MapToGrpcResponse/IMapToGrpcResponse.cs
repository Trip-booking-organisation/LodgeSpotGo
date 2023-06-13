using JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Domain.Reservation;

namespace JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;

public interface IMapToGrpcResponse
{
    Task<GetReservationListResponse> MapSearchToGrpcResponse(List<SearchReservationResponse> list);
    Task<GetReservationsByGuestIdResponse> MapGetByGuestIdToGrpcResponse(List<GetReservationsByGuestIdCommandResponse> list);
    Task<GetReservationByAccommodationResponse> MapGetByAccommodationToGrpcResponse(List<Reservation> list);
    Task<GetReservationByGuestAndAccomResponse> MapGetByGuestAndAccommodationToGrpcResponse(List<Reservation> list);

    GetDeletedReservationsByGuestResponse MapDeletedCountToGrpcResponse(List<Reservation> reservations);
}