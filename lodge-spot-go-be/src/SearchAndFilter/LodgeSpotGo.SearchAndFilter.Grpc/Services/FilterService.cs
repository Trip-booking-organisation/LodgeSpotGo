using System.Diagnostics;
using Google.Protobuf.Collections;
using Grpc.Core;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.FilterAverageGradeAccommodation;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.User;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Services;

public class FilterService : FilterApp.FilterAppBase
{
    private readonly IFilterAverageGradeClient _gradeClient;
    private readonly IUserClient _userClient;
    public static readonly ActivitySource ActivitySource = new("Filter activity");

    public FilterService(IFilterAverageGradeClient gradeClient, IUserClient userClient)
    {
        _gradeClient = gradeClient;
        _userClient = userClient;
    }

    public override Task<FilterReservationListResponse> Filter(ReservationFilterRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var filteredByAmenities = FilterAccommodationsByAmenities(request.Filter);
        if (request.Filter.OutstandingHost)
        {
            filteredByAmenities =  FilterOutstandingHost(filteredByAmenities);
        }
        filteredByAmenities = FilterByGrades(request, filteredByAmenities);
       
        var response = new FilterReservationListResponse();
        response.Accommodations.AddRange(filteredByAmenities);
        activity?.Stop();
        return Task.FromResult(response);
    }

    private RepeatedField<AccommodationDto> FilterOutstandingHost(RepeatedField<AccommodationDto> filteredByAmenities)
    {
        var filterByHost = filteredByAmenities;
        foreach (var accom in filteredByAmenities)
        {
            var isOutstanding = _userClient.IsHostOutstanding(Guid.Parse(accom.HostId));
            if (!isOutstanding.IsOutstanding)
                filterByHost.Remove(accom);
        }

        return filterByHost;
    }

    private RepeatedField<AccommodationDto> FilterByGrades(ReservationFilterRequest request, RepeatedField<AccommodationDto> filteredByAmenities)
    {
        var filterByGrades = filteredByAmenities;
        foreach (var accommodation in filteredByAmenities)
        {
            var averageGrade = _gradeClient.GetAverageGradeForAccommodation(Guid.Parse(accommodation.Id))
                .AverageGradeNumber;
            if (request.Filter.MinGrade > averageGrade || averageGrade > request.Filter.MaxGrade)
            {
                filterByGrades.Remove(accommodation);
            }
        }

        return filterByGrades;
    }

    private RepeatedField<AccommodationDto> FilterAccommodationsByAmenities(FilterAccommodations requestFilter)
    {
        var amenities = requestFilter.Amenities;
        var accommodations = requestFilter.Accommodations;
        var filteredAccommodations = accommodations;
        foreach (var accommodation in accommodations)
        {
            var remove = false;
            foreach (var amenity in amenities)
            {
                if (!accommodation.Amenities.Contains(amenity))
                    remove = true;
            }

            if (remove)
                filteredAccommodations.Remove(accommodation);
        }

        return filteredAccommodations;
    }
}