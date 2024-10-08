﻿using JetSetGo.ReservationManagement.Application.CancelReservation;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;

namespace JetSetGo.ReservationManagement.Application.Common.Persistence;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task  CreateAsync(Reservation reservation);
    Task<List<Reservation>> SearchReservations(DateRange request);
    Task CancelReservation(Reservation request);
    Task<Reservation> GetById(Guid id,CancellationToken cancellationToken = default);
    Task UpdateReservationStatus(Reservation reservation);
    Task<List<Reservation>> GetByAccommodationId(Reservation reservation);
    Task<List<Reservation>> GetByGuestId(Guid guestId);
    Task<List<Reservation>> GetReservationsByAccommodation(Guid accommodationId);
    Task<List<Reservation>> GetReservationsAllByAccommodation(Guid accommodationId);
    Task<List<Reservation>> GetDeletedByGuest(Guid guestId);
    Task DeleteReservation(Guid requestId);
    Task<List<Reservation>> GetByGuestIdConfirmed(Guid guestId);
    Task<List<Reservation>> GetByGuestAndAccommodation(Guid guestId,Guid accommodationId);
}