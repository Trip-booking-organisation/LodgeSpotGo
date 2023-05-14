using JetSetGo.AccommodationManagement.Domain.Accommodation.Enum;
using JetSetGo.AccommodationManagement.Domain.Accommodation.ValueObjects;

namespace JetSetGo.AccommodationManagement.Domain.Accommodation;

public class Accommodation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public List<string> Amenities { get; set; } = null!;
    public List<AccommodationPhoto> Photos { get; set; } = null!;
    public int MaxGuests { get; set; }
    public int MinGuests { get; set; }
    
    public List<SpecalPrice> SpecalPrices { get; set; }
}