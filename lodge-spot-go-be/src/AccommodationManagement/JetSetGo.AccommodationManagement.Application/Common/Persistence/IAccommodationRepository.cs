﻿using JetSetGo.AccommodationManagement.Application.SearchAccommodation;
using JetSetGo.AccommodationManagement.Domain.Accommodations;

namespace JetSetGo.AccommodationManagement.Application.Common.Persistence;

public interface IAccommodationRepository
{
    Task<List<Accommodation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Accommodation> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Accommodation Get(Guid id);
    Task CreateAsync(Accommodation accommodation);
    Task UpdateAsync(Accommodation accommodation);
    Task RemoveAsync(Guid id);
    Task<List<Accommodation>> SearchAccommodations(SearchAccommodationQuery searchAccommodationQuery);
    Task<List<Accommodation>> GetByHost(Guid hostId);
}