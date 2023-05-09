using Grpc.Core;
using MediatR;

namespace JetSetGo.AccommodationManagement.Grpc.Services;

public class AccommodationService : AccommodationApp.AccommodationAppBase
{
    private readonly ILogger<AccommodationService> _logger;
    private readonly ISender _sender;

    public AccommodationService(ILogger<AccommodationService> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }
    public override  Task<GetBookListResponse> GetAccommodationList(GetAccommodationListRequest request, ServerCallContext context)
    {
        var response = new GetBookListResponse();
        response.Accommodations.Add(new AccommodationDto
        {
            Amenities = "iva",
            Location = "iva",
            Name = "iva",
            MaxGuests = 2
        });
        return Task.FromResult(response);
    }
    
    public override Task<CreateAccommodationResponse> CreateAccommodation(CreateAccommodationRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"Success {request.Accommodation}",request.Accommodation );
        return Task.FromResult(new CreateAccommodationResponse
        {
            Created = "true"
        });
    }
}