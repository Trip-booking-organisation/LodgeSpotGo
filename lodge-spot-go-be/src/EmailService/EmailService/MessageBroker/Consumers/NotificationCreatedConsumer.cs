using EmailService.Service;
using LodgeSpotGo.Shared.Events.Email;
using LodgeSpotGo.Shared.Events.Notification;
using MassTransit;

namespace EmailService.MessageBroker.Consumers;

public class NotificationCreatedConsumer : IConsumer<SendEmailCommand>
{
    private readonly ILogger<NotificationCreatedConsumer> _logger;
    private readonly IEmailService _emailService;

    public NotificationCreatedConsumer(ILogger<NotificationCreatedConsumer> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        _logger.LogInformation("Received email request {}",context.Message.Email);
        var emailStatus = await _emailService.Send(context.Message.Subject, context.Message.Content, context.Message.Email);
        var @event = emailStatus ? new EmailSendStatus
        {
            IsSuccess = emailStatus,
            FailureDetails = "Provided email is not valid!"
        } : new EmailSendStatus {
            IsSuccess = emailStatus
        };
        await context.RespondAsync(@event);
    }
}