using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using MediatR;

namespace JetSetGo.ReservationManagement.Application.CancelReservation;

public class CancelReservationHandler : IRequestHandler<CancelReservationCommand,CancelReservationCommandResponse>
{
    private readonly IReservationRepository _reservationRepository;

    public CancelReservationHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<CancelReservationCommandResponse> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetById(request.Id,cancellationToken);
       if(reservation.ReservationStatus != ReservationStatus.Confirmed)
           return new CancelReservationCommandResponse{Success = false};
       if (!ValidateDate(reservation.DateRange.From)) 
           return new CancelReservationCommandResponse {Success = false};
       await _reservationRepository.CancelReservation(reservation);
       return new CancelReservationCommandResponse {Success = true};
    }

    private bool ValidateDate(DateTime from)
    {
        return (from - DateTime.Now).TotalDays > 1;
    }
}