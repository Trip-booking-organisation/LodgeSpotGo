namespace JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;

public class Grade
{
    public Guid Id { get; set; }
    public Guid AccommodationId { get; set; }
    public Guid GuestId { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
}