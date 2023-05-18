
using Grpc.Core;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Grpc.Clients;

namespace JetSetGo.ReservationManagement.Grpc.Services;

public class UserReservationService : UserServiceGrpc.UserServiceGrpcBase
{
    private readonly ILogger<UserReservationService> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IGetAccommodationClient _accommodationClient;

    public UserReservationService(ILogger<UserReservationService> logger, IReservationRepository reservationRepository, IGetAccommodationClient accommodationClient)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _accommodationClient = accommodationClient;
    }

    public override async Task<ReservationResponse> GetUserReservations(UserRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"---------------- {}",request.UserId);
        switch (request.Role)
        {
            case "guest":
                var list = await _reservationRepository.GetByGuestIdConfirmed(new Guid(request.UserId));
                if (list.Any())
                {
                    return new ReservationResponse
                    {
                        HasReservation = true
                    }; 
                }
                return new ReservationResponse
                {
                    HasReservation = false
                };
            case "host":
                var response = new ReservationResponse
                {
                    HasReservation = false
                };
                var reservations = await _reservationRepository.GetAllAsync();
                foreach (var r in reservations)
                {
                    if (r.ReservationStatus != ReservationStatus.Confirmed)
                    {
                        continue;
                    };
                    var accommodationResponse = _accommodationClient
                        .GetAccommodation(new GetAccommodationRequest {Id = r.AccommodationId.ToString()});
                    if (accommodationResponse.Accommodation.HostId == request.UserId)
                    {
                        response.HasReservation = true;
                        break;
                    }
                }

                return response;

        }
        return new ReservationResponse
        {
            HasReservation = true
        };
    }
}