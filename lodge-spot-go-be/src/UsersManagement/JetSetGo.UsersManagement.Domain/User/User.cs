namespace JetSetGo.UsersManagement.Domain.User;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}