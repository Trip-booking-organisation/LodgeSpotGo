namespace LodgeSpotGo.Shared.Events.Email;

public class SendEmailCommand
{
    public string Content { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Subject { get; set; } = null!;
}