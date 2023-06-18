using Grpc.Core;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;

namespace JetSetGo.ReservationManagement.Grpc.Services;

public class GetReservationByAccomAndGuestService : GetReservationApp.GetReservationAppBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapToGrpcResponse _mapToGrpcResponse;
    private readonly ILogger<GetReservationByAccomAndGuestService> _logger;


    public GetReservationByAccomAndGuestService(IReservationRepository reservationRepository, IMapToGrpcResponse mapToGrpcResponse, ILogger<GetReservationByAccomAndGuestService> logger)
    {
        _reservationRepository = reservationRepository;
        _mapToGrpcResponse = mapToGrpcResponse;
        _logger = logger;
    }
    public override async Task<GetReservationByGuestAndAccomResponse> GetReservationByGuestAndAccomRequest(GetReservationByGuestAndAccom request, ServerCallContext context)
    {
        _logger.LogInformation(@"Request came in !!!!!!!!!!!");
        var reservations = await _reservationRepository.GetByGuestAndAccommodation(Guid.Parse(request.GuestId),
            Guid.Parse(request.AccommodationId));
        var result = _mapToGrpcResponse.MapGetByGuestAndAccommodationToGrpcResponse(reservations);
        return await result;
    }
}