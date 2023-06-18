using JetSetGo.ReservationManagement.Domain.Reservation;

namespace JetSetGo.ReservationManagement.Grpc.Handlers;

public class CreateReservationResponse
{
    public Reservation Reservation { get; set; } = null!;
    public GetAccommodationResponse AccommodationResponse { get; set; } = null!;
}