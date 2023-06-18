using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Shared.Events.Host;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Consumers;

public class OutstandingHostStatus: IConsumer<OutstandingHostStatusChanged>
{
    private readonly ILogger<OutstandingHostStatus> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly IHostNotificationRepository _repository;

    public OutstandingHostStatus(
        ILogger<OutstandingHostStatus> logger, 
        IHubContext<NotificationsHub> hubContext, 
        IHostNotificationRepository repository)
    {   
        _logger = logger;
        _hubContext = hubContext;
        _repository = repository;
    }

    public async Task  Consume(ConsumeContext<OutstandingHostStatusChanged> context)
    {
        _logger.LogInformation(@"--------- Host id ---------{}",context.Message.HostId);
        var notification = context.Message.IsOutstanding
            ? GetOutstandingHostStatus(context.Message)
            : LoseOutstandingHostStatus(context.Message);
        await _repository.CreateAsync(notification);
        var notifications = await _repository.GetAllNotificationsByHost(context.Message.HostId);
        var mapped = NotificationsMapper.MapNotificationsHost(notifications);
        await _hubContext.Clients.All.SendAsync($"ReceiveNotification/{context.Message.HostId}", mapped);
    }

    private HostNotification LoseOutstandingHostStatus(OutstandingHostStatusChanged message)
    {
        var content = $"You have lost the status of an outstanding host as of that date {message.CreatedAt}";
        var notification = new HostNotification
        {
            HostId = message.HostId,
            Content = content,
            HostNotificationType = HostNotificationType.LoseOutstandingHostStatus,
            CreatedAt = message.CreatedAt
        };
        return notification;
    }
    private HostNotification GetOutstandingHostStatus(OutstandingHostStatusChanged message)
    {
        var content = $"Congratulations! You have become an outstanding host as of date {message.CreatedAt}";
        var notification = new HostNotification
        {
            HostId = message.HostId,
            Content = content,
            HostNotificationType = HostNotificationType.BecomeOutstandingHost,
            CreatedAt = message.CreatedAt
        };
        return notification;
    }
}