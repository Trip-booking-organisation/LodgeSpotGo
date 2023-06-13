namespace JetSetGo.UsersManagement.Domain.HostGrade.Entities;

public class HostGrade
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public Guid GuestId { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
}