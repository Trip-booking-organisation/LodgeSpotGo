using LodgeSpotGo.Notifications.Api.Hubs;
using LodgeSpotGo.Notifications.Api.Mappers;
using LodgeSpotGo.Notifications.Core.Common.Interfaces.Repository;
using LodgeSpotGo.Notifications.Core.Notifications;
using LodgeSpotGo.Shared.Events.Grades;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace LodgeSpotGo.Notifications.Api.Consumers;

public class CreatedAccommodationGradeConsumer : IConsumer<AccommodationGradeCreated>
{
    private readonly ILogger<CreatedAccommodationGradeConsumer> _logger;
    private readonly IHostNotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationsHub> _hubContext;
    public CreatedAccommodationGradeConsumer(
        ILogger<CreatedAccommodationGradeConsumer> logger, 
        IHostNotificationRepository notificationRepository, 
        IHubContext<NotificationsHub> hubContext)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
    }
    public async Task Consume(ConsumeContext<AccommodationGradeCreated> context)
    {
        _logger.LogInformation("---------- Created Accommodation Grade Event Guest------ {}", context.Message.GuestEmail);
        var content = $@"User with email {context.Message.GuestEmail} has rated your accommodation 
                {context.Message.AccommodationName} with grade {context.Message.Grade}  on date: {context.Message.CreatedAt}.";
        var notification = new HostNotification
        {
            HostId = context.Message.HostId,
            Content = content,
            HostNotificationType = HostNotificationType.CreatedAccommodationGrade,
            CreatedAt = context.Message.CreatedAt
        };
        await _notificationRepository.CreateAsync(notification);
        var notifications = await _notificationRepository.GetAllNotificationsByHost(context.Message.HostId);
        var mapped = NotificationsMapper.MapNotificationsHost(notifications);
        await _hubContext.Clients.All.SendAsync($"ReceiveNotification/{context.Message.HostId}", mapped);
    }
}