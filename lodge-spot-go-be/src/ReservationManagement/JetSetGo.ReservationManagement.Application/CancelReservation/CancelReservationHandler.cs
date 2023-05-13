using JetSetGo.ReservationManagement.Application.Common.Persistence;
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
        //TODO Check if reservation is confirmed!!!
       var reservation = await _reservationRepository.GetById(request.Id,cancellationToken);
       if (ValidateDate(reservation.DateRange.From))
       {
           await _reservationRepository.CancelReservation(request);
           return new CancelReservationCommandResponse {Success = true};
       }
       return new CancelReservationCommandResponse{Success = false};
    }

    private bool ValidateDate(DateTime from)
    {
        return (from - DateTime.Now).TotalDays > 1;
    }
}