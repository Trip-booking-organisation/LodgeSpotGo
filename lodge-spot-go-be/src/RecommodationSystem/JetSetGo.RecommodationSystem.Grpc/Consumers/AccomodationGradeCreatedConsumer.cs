using LodgeSpotGo.RecommodationSystem.Core.Model;
using LodgeSpotGo.RecommodationSystem.Core.Services;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;
using LodgeSpotGo.Shared.Events.Grades;
using MassTransit;

namespace JetSetGo.RecommodationSystem.Grpc.Consumers;

public class AccomodationGradeCreatedConsumer : IConsumer<AccommodationGradeCreated>
{
    private readonly ILogger<CreatedReservationConsumer> _logger;
    private readonly RecommendationService recommendationService;

    public AccomodationGradeCreatedConsumer(ILogger<CreatedReservationConsumer> logger, IRecommodationRepository recommodationRepository, RecommendationService recommendationService)
    {
        _logger = logger;
        this.recommendationService = recommendationService;
    }

    public async Task Consume(ConsumeContext<AccommodationGradeCreated> context)
    {
        _logger.LogInformation(@"Accommodation grade event {}",context.Message.GuestEmail);
        var guest = new Guest
        {
            Name = context.Message.GuestEmail
        };
        var acc = new Accommodation
        {
            Id = context.Message.AccommodationId.ToString(),
            Name = context.Message.AccommodationName
        };
        await recommendationService.MakeAccommodationGrade(context.Message.Grade, guest, acc);
    }
}