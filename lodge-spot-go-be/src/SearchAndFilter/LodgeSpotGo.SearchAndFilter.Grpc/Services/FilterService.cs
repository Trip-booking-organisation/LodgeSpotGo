using Google.Protobuf.Collections;
using Grpc.Core;
using LodgeSpotGo.SearchAndFilter.Grpc.Clients.FilterAverageGradeAccommodation;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Services;

public class FilterService : FilterApp.FilterAppBase
{
    private readonly IFilterAverageGradeClient _gradeClient;

    public FilterService(IFilterAverageGradeClient gradeClient)
    {
        _gradeClient = gradeClient;
    }

    public override Task<FilterReservationListResponse> Filter(ReservationFilterRequest request, ServerCallContext context)
    {
        var filteredByAmenities = FilterAccommodationsByAmenities(request.Filter);
        var filterByGrades = FilterByGrades(request, filteredByAmenities);
        var response = new FilterReservationListResponse();
        response.Accommodations.AddRange(filterByGrades);
        return Task.FromResult(response);
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