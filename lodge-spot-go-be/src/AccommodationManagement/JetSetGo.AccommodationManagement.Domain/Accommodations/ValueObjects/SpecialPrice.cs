namespace JetSetGo.AccommodationManagement.Domain.Accommodations.ValueObjects;

public class SpecialPrice
{
    public DateRange DateRange { get; set; } = null!;
    public double Price { get; set; }
}