using JetSetGo.AccommodationManagement.Domain.Accommodation.ValueObjects;

namespace JetSetGo.AccommodationManagement.Application.SearchAccommodation;

public class SearchAccommodationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public List<string> Amenities { get; set; } = null!;
    public List<AccommodationPhoto> Photos { get; set; } = null!;
    public int MaxGuests { get; set; }
    public int MinGuests { get; set; }
}
