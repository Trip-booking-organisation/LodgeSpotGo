namespace JetSetGo.AccommodationManagement.Application.Dto.Response;

public class UserResponse
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Guid Id { get; set; }
}