namespace LodgeSpotGo.Shared.Events.Email;

public class EmailSendStatus
{
    public bool IsSuccess { get; set; }
    public string? FailureDetails { get; set; }
}