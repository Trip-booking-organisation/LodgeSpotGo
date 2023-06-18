using Grpc.Core;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;

namespace JetSetGo.ReservationManagement.Grpc.Services;

public class ReservationAccommodationHost :GetReservationAccommodationApp.GetReservationAccommodationAppBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapToGrpcResponse _mapToGrpcResponse;

    public ReservationAccommodationHost(IReservationRepository reservationRepository, IMapToGrpcResponse mapToGrpcResponse)
    {
        _reservationRepository = reservationRepository;
        _mapToGrpcResponse = mapToGrpcResponse;
    }

    public override async Task<GetReservationAccommodationHostResponse> GetReservationByGuestAndAccomRequest(GetReservationAccommodationRequest request, ServerCallContext context)
    {
        
        var reservations = await _reservationRepository.GetReservationsAllByAccommodation(Guid.Parse(request.AccommodationId));
        var response = new GetReservationAccommodationHostResponse();
        var result = _mapToGrpcResponse.MapToHostResponse(reservations);
        return await result;
    }
}