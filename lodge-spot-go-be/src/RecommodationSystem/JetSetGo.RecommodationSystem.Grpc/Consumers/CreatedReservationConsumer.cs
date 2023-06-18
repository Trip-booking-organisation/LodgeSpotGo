using LodgeSpotGo.RecommodationSystem.Core.Model;
using LodgeSpotGo.RecommodationSystem.Core.Services;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;
using LodgeSpotGo.Shared.Events.Reservation;
using MassTransit;

namespace JetSetGo.RecommodationSystem.Grpc.Consumers;

public class CreatedReservationConsumer : IConsumer<CreatedReservationEvent>
{
    
    private readonly ILogger<CreatedReservationConsumer> _logger;
    private readonly RecommendationService recommendationService;

    public CreatedReservationConsumer(ILogger<CreatedReservationConsumer> logger, IRecommodationRepository recommodationRepository, RecommendationService recommendationService)
    {
        _logger = logger;
        this.recommendationService = recommendationService;
    }
    public async Task Consume(ConsumeContext<CreatedReservationEvent> context)
    {
        _logger.LogInformation(@"Created reservation event {}",context.Message.GuestEmail);
        var guest = new Guest
        {
            Name = context.Message.GuestEmail
        };
        var acc = new Accommodation
        {
            Id = context.Message.AccommodationId,
            Name = context.Message.AccommodationName
        };
        await recommendationService.MakeReservation(guest, acc);
        var bla = await recommendationService.GetRecommendedAccommodations(guest);
        // _logger.LogInformation(@"Recommendeeeeeeeeed {}",bla.ToString());
        await Task.CompletedTask;
    }
}