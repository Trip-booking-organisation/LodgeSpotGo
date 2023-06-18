using JetSetGo.ReservationManagement.Application.Clients;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Application.MessageBroker;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.Enums;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;
using LodgeSpotGo.Shared.Events.Reservation;
using MediatR;

namespace JetSetGo.ReservationManagement.Application.UpdateReservationStatus;

public class UpdateReservationStatusHandler : IRequestHandler<UpdateReservationStatusCommand,UpdateReservationStatusCommandResponse>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventBus _eventBus;
    private readonly IClientAccommodationMediator _accommodationMediator;

    public UpdateReservationStatusHandler(
        IReservationRepository reservationRepository, 
        IEventBus eventBus, 
        IClientAccommodationMediator accommodationMediator)
    {
        _reservationRepository = reservationRepository;
        _eventBus = eventBus;
        _accommodationMediator = accommodationMediator;
    }

    public async Task<UpdateReservationStatusCommandResponse> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetById(request.Id, cancellationToken);
        reservation.ReservationStatus =  MapStringToReservationStatus(request.Status);
        await _reservationRepository.UpdateReservationStatus(reservation);
        var accommodation = _accommodationMediator.GetAccommodation(reservation.AccommodationId);
        var @event = new ReservationStateChangedEvent
        {
            AccommodationName = accommodation.Name,
            NewStatus = request.Status,
            GuestId = reservation.GuestId,
            From = reservation.DateRange.From,
            To = reservation.DateRange.To,
            CreatedAt = DateTime.Now
        };
        await _eventBus.PublishAsync(@event,cancellationToken);
        var reservations = await _reservationRepository.GetByAccommodationId(reservation);
        if (request.Status.Equals("Confirmed"))
            await RefuseReservations(reservations,reservation.DateRange,cancellationToken);
        return new UpdateReservationStatusCommandResponse{Success = true};
    }

    private async Task RefuseReservations(List<Reservation> reservations, DateRange resultDateRange,
        CancellationToken cancellationToken)
    {
        foreach (var reservation in reservations)
        {
            if (!reservation.IsOverlapping(resultDateRange)) continue;
            reservation.ReservationStatus = ReservationStatus.Refused;
            await _reservationRepository.UpdateReservationStatus(reservation);
            var @event = new ReservationStateChangedEvent
            {
                NewStatus = "Refused",
                GuestId = reservation.GuestId,
                From = reservation.DateRange.From,
                To = reservation.DateRange.To,
                CreatedAt = DateTime.Now
            };
            await _eventBus.PublishAsync(@event,cancellationToken);
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