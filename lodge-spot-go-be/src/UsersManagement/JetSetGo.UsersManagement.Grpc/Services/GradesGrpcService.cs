using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Domain.HostGrade.Entities;
using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Dto;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class GradesGrpcService
{
    private readonly IHostGradeRepository _hostGradeRepository;
    private readonly IMapper _mapper;
    private readonly IReservationClient _reservationClient;
    public GradesGrpcService(IHostGradeRepository gradeRepository, IMapper mapper, IReservationClient reservationClient)
    {
        _hostGradeRepository = gradeRepository;
        _mapper = mapper;
        _reservationClient = reservationClient;
    }

    public async Task<HostGradeResponse> CreateGradeForHost(HostGradeRequest request)
    {
        var reservations = _reservationClient
            .GetReservationsByGuestAndHostId(request.GuestId,
                request.AccomodationId);
        if (!CheckIfGuestHasStayedInAccommodation(reservations.Reservations))
            throw new RpcException(new Status(StatusCode.Cancelled, "You can't grade this accommodation!"));
        var grade = new HostGrade
        {
            GuestId = request.GuestId,
            Date = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day),
            Number = request.Number,
            HostId = request.HostId
        };
        await _hostGradeRepository.CreateGrade(grade);
        return new HostGradeResponse { success = true };
    }
    
    private bool CheckIfGuestHasStayedInAccommodation( RepeatedField<GetReservationDto> reservations)
    {
        return reservations.Any(reservation => reservation.DateRange.To.ToDateTime() < DateTime.Now);
    }
}