using JetSetGo.AccommodationManagement.Domain.Accommodations.ValueObjects;

namespace JetSetGo.AccommodationManagement.Application.SearchAccommodation;

public class SearchAccommodationResponse
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public string Name { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public List<string> Amenities { get; set; } = null!;
    public List<AccommodationPhoto> Photos { get; set; } = null!;
    public int MaxGuests { get; set; }
    public int MinGuests { get; set; }
    public List<SpecialPrice> SpecalPrices { get; set; } = null!;
}
