using AutoMapper;
using JetSetGo.ReservationManagement.Application.GetReservationsByGuestId;
using JetSetGo.ReservationManagement.Application.SearchReservations;
using JetSetGo.ReservationManagement.Domain.Reservation;

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

    public Task<GetReservationByAccommodationResponse> MapGetByAccommodationToGrpcResponse(List<Reservation> list)
    {
        var response = new GetReservationByAccommodationResponse();
        var responseList = list.Select(reservation => _mapper.Map<ReadReservationDto>(reservation)).ToList();
        responseList.ForEach(dto => response.Reservations.Add(dto));
        return Task.FromResult(response);
    }
    public Task<GetReservationByGuestAndAccomResponse> MapGetByGuestAndAccommodationToGrpcResponse(List<Reservation> list)
    {
        var response = new GetReservationByGuestAndAccomResponse();
        var responseList = list.Select(reservation => _mapper.Map<GetReservationDto>(reservation)).ToList();
        responseList.ForEach(dto => response.Reservations.Add(dto));
        return Task.FromResult(response);
    }

    public GetDeletedReservationsByGuestResponse MapDeletedCountToGrpcResponse(List<Reservation> list)
    {
        var response = new GetDeletedReservationsByGuestResponse
        {
            Count = list.Count
        };
        return response;
    }

 
}