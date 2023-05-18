namespace JetSetGo.AccommodationManagement.Domain.Accommodation.ValueObjects;

public class SpecialPrice
{
    public DateRange DateRange { get; set; } = null!;
    public double Price { get; set; }
}