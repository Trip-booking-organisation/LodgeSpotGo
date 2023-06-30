using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailService.Service;

public class SendEmailService : IEmailService
{
    private ILogger<SendEmailService> _logger;

    public SendEmailService(ILogger<SendEmailService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> Send(string subject, string content, string email)
    {
        var client = new SendGridClient("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");
        var from = new EmailAddress("ivanalazarevic01@gmail.com", "Lodge Spot Go");
        var to = new EmailAddress(email);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, null, BuildHtmlContent(content));
        var response = await client.SendEmailAsync(msg);
        
        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            // Handle error if email sending fails
            _logger.LogInformation("Failed to send email. Status code: {}",response.StatusCode);
            return false;
        }
        return true;
    }

    private static string BuildHtmlContent(string content)
    {
        return  $@"<html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #3D5CB8FF;
                        }}
                        h1 {{
                            color: #3D5CB8FF;
                        }}
                        .message-container {{
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 15px;
                            box-shadow: 0px 0px 5px rgba(0, 0, 0, 0.1);
                            color: #3D5CB8FF;
                        }}
                        .message-content {{
                            color: #3D5CB8FF;
                        }}
                        .thank-you {{
                            color: #3D5CB8FF;
                            margin-top: 20px;
                        }}
                        .btn {{
                            display: inline-block;
                            border-radius: 15px;
                            background-color: #757575FF;
                            color: white;
                            padding: 10px 20px;
                            text-decoration: none;
                        }}
                        .btn:hover {{
                           background-color: #202639FF;
                        }}

                    </style>
                </head>
                <body>
                    <div class='message-container'>
                        <h1>Reservation Confirmation</h1>
                        <p class='message-content'>Reservation submitted successfully!</p>
                        <p class='message-content'>{content}</p>
                        <a href='http://localhost:4200/reservations' class='btn'>Visit us</button>
                    </div>
                    <p class='thank-you'>Thank you for choosing our service!</p>
                </body>
            </html>";
    }
}