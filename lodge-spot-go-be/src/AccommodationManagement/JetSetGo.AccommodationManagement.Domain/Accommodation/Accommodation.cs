using JetSetGo.AccommodationManagement.Domain.Accommodation.Enum;

namespace JetSetGo.AccommodationManagement.Domain.Accommodation;

public class Accommodation
{
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public Amenity Amenity { get; set; }
    public int MaxGuests { get; set; }
}