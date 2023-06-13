using Grpc.Core;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;

namespace JetSetGo.ReservationManagement.Grpc.Services;

public class GetReservationByAccomAndGuestService : GetReservationApp.GetReservationAppBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapToGrpcResponse _mapToGrpcResponse;
   

    public GetReservationByAccomAndGuestService(IReservationRepository reservationRepository, IMapToGrpcResponse mapToGrpcResponse)
    {
        _reservationRepository = reservationRepository;
        _mapToGrpcResponse = mapToGrpcResponse;
    }
    public override async Task<GetReservationByGuestAndAccomResponse> GetReservationByGuestAndAccomRequest(GetReservationByGuestAndAccom request, ServerCallContext context)
    {
        var reservations = await _reservationRepository.GetByGuestAndAccommodation(Guid.Parse(request.GuestId),
            Guid.Parse(request.AccommodationId));
        var result = _mapToGrpcResponse.MapGetByGuestAndAccommodationToGrpcResponse(reservations);
        return await result;
    }
}