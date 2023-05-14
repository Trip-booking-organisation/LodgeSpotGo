using Grpc.Core;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;
using MediatR;

namespace JetSetGo.ReservationManagement.Grpc.Services;

public class SearchReservationService : SearchReservationApp.SearchReservationAppBase
{
    private readonly ISender _sender;
    private readonly ILogger<SearchReservationService> _logger;
    private readonly IMapToGrpcResponse _mapToGrpcResponse;

    public SearchReservationService(ISender sender, ILogger<SearchReservationService> logger, IMapToGrpcResponse mapToGrpcResponse)
    {
        _sender = sender;
        _logger = logger;
        _mapToGrpcResponse = mapToGrpcResponse;
    }

    public override async Task<GetReservationListResponse> SearchReservations(ReservationSearchRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"----------------Request came in to RESERVATION {}", request.ToString());
        var searchReservationQuery =
            new SearchReservationsQuery(request.StartDate.ToDateTime(), request.EndDate.ToDateTime());
        var reservationsResult = await _sender.Send(searchReservationQuery);
        _logger.LogInformation(@"----------------------Response from DB : {}",reservationsResult.Count);
        var result = _mapToGrpcResponse.MapSearchToGrpcResponse(reservationsResult);
        return await result;
    }
}