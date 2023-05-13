using System;

namespace LodgeSpotGo.SearchAndFilter.Grpc.Dto.Requests;

public class SearchRequest
{
    public int NumberOfGuests { get; set; }
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}