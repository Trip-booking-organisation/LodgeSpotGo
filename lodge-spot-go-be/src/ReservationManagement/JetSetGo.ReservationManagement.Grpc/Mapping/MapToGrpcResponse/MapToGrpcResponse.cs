using AutoMapper;
using JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;
using JetSetGo.ReservationManagement.Application.SearchReservations;

namespace JetSetGo.ReservationManagement.Grpc.Mapping.MapToGrpcResponse;

public class MapToGrpcResponse : IMapToGrpcResponse
{
    private readonly IMapper _mapper;

    public MapToGrpcResponse(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<GetReservationListResponse> MapSearchToGrpcResponse(List<SearchReservationResponse> list)
    {
        var response = new GetReservationListResponse();
        var responseList = list.Select(reservation => _mapper.Map<ReadReservationDto>(reservation)).ToList();
        responseList.ForEach(dto => response.Reservations.Add(dto));
        return Task.FromResult(response);
    }

    public Task<GetReservationsByGuestIdResponse> MapGetByGuestIdToGrpcResponse(List<GetReservationsByGuestIdCommandResponse> list)
    {
        var response = new GetReservationsByGuestIdResponse();
        var responseList = list.Select(reservation => _mapper.Map<ReadReservationDto>(reservation)).ToList();
        responseList.ForEach(dto => response.Reservations.Add(dto));
        return Task.FromResult(response);
    }
}