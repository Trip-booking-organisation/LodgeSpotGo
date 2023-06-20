namespace EmailService.Service;

public interface IEmailService
{
    Task<bool> Send(string subject, string content, string email);
}