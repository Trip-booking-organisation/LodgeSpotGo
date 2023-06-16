namespace LodgeSpotGo.Shared.Events.Grades;

public class HostGradeCreatedEvent
{
    public Guid GuestId { get; set; }
    public string GuestEmail { get; set; } = null!;
    public int Grade { get; set; }
    public Guid HostId { get; set; }
    public DateTime CreatedAt { get; set;}
}