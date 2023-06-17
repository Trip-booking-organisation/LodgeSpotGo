using LodgeSpotGo.RecommodationSystem.Core.Model;
using LodgeSpotGo.RecommodationSystem.Core.Services;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;
using LodgeSpotGo.Shared.Events.Accommdation;
using MassTransit;

namespace JetSetGo.RecommodationSystem.Grpc.Consumers;

public class AccommodationCreatedConsumer: IConsumer<AccommodationCreatedEvent>
{
    private readonly ILogger<AccommodationCreatedConsumer> _logger;
    private readonly RecommendationService recommendationService;

    public AccommodationCreatedConsumer(ILogger<AccommodationCreatedConsumer> logger, RecommendationService recommendationService)
    {
        _logger = logger;
        this.recommendationService = recommendationService;
    }

    public async Task Consume(ConsumeContext<AccommodationCreatedEvent> context)
    {
        _logger.LogInformation(@"Created accommodation event {}",context.Message.Name);
        var accommodation = new Accommodation
        {
            Name = context.Message.Name,
            Id = context.Message.Id
        };
        await recommendationService.CreateAccommodation(accommodation);
        await Task.CompletedTask;

    }
}