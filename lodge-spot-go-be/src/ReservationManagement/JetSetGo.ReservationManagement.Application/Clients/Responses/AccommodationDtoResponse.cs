namespace JetSetGo.ReservationManagement.Application.Clients.Responses;

public class AccommodationDtoResponse
{
    public string Id { get; set; }= string.Empty;
    public string Name { get; set; }= string.Empty;
    public AddressDto Address { get; set; }= new();
    public List<string> Amenities { get; set; }= new();
    public int MaxGuests { get; set; }
    public int MinGuests { get; set; }
    public List<string> Photos { get; set; } = new();
    public List<SpecialPriceDto> SpecialPrices { get; set; }= new();
    public string HostId { get; set; }= string.Empty;
    public bool AutomaticConfirmation { get; set; }
}
public class AddressDto
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; }= string.Empty;
    public string Street { get; set; }= string.Empty;
}

public class PhotoDto
{
    public string Photo { get; set; }= string.Empty;
}

public class SpecialPriceDto
{
    public DateRangeDto DateRange { get; set; }= new();
    public double Price { get; set; }
}

public class DateRangeDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}