namespace LodgeSpotGo.Shared.Events.Grades;

public class AccommodationGradeCreated
{
    public int Grade { get; set; }
    public Guid AccommodationId { get; set; }
    public string AccommodationName { get; set; } = null!;
    public Guid GuestId { get; set; }
    public Guid HostId { get; set; }
    public string GuestEmail { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}