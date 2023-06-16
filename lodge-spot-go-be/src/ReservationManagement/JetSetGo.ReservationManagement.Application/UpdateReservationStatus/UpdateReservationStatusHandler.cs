using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.MessageBroker;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using MediatR;

namespace JetSetGo.ReservationManagement.Application.UpdateReservationStatus;

public class UpdateReservationStatusHandler : IRequestHandler<UpdateReservationStatusCommand,UpdateReservationStatusCommandResponse>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventBus _eventBus;

    public UpdateReservationStatusHandler(IReservationRepository reservationRepository, IEventBus eventBus)
    {
        _reservationRepository = reservationRepository;
        _eventBus = eventBus;
    }

    public async Task<UpdateReservationStatusCommandResponse> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
    {
        var reservation = _reservationRepository.GetById(request.Id, cancellationToken);
        reservation.Result.ReservationStatus =  MapStringToReservationStatus(request.Status);
        await _reservationRepository.UpdateReservationStatus(reservation.Result);
        var reservations = _reservationRepository.GetByAccommodationId(reservation.Result);
        if (request.Status.Equals("Confirmed"))
            await RefuseReservations(reservations,reservation.Result.DateRange);
        return new UpdateReservationStatusCommandResponse{Success = true};
    }

    private async Task RefuseReservations(Task<List<Reservation>> reservations, DateRange resultDateRange)
    {
        foreach (var reservation in reservations.Result)
        {
            if (!reservation.IsOverlapping(resultDateRange)) continue;
            reservation.ReservationStatus = ReservationStatus.Refused;
            await _reservationRepository.UpdateReservationStatus(reservation);
        }
    }

    private ReservationStatus MapStringToReservationStatus(string requestStatus)
    {
        return requestStatus switch
        {
            "Confirmed" => ReservationStatus.Confirmed,
            "Refused" => ReservationStatus.Refused,
            _ => ReservationStatus.Waiting
        };
    }
}