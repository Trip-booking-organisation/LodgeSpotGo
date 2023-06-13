namespace JetSetGo.UsersManagement.Domain.User.Entities;

public class Grade
{
    public Guid HostId { get; set; }
    public Guid GuestId { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
}