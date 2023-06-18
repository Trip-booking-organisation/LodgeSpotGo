using AutoMapper;
using FluentResults;
using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Application.MessageBroker;
using JetSetGo.UsersManagement.Domain.HostGrade.Entities;
using JetSetGo.UsersManagement.Domain.User.Entities;
using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Dto;
using JetSetGo.UsersManagement.Grpc.Dto.Request;
using JetSetGo.UsersManagement.Grpc.Dto.Response;
using LodgeSpotGo.Shared.Events.Grades;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class GradesGrpcService
{
    private readonly IHostGradeRepository _hostGradeRepository;
    private readonly IMapper _mapper;
    private readonly IReservationClient _reservationClient;
    private readonly IEventBus _eventBus;
    public GradesGrpcService(
        IHostGradeRepository gradeRepository, 
        IMapper mapper,
        IReservationClient reservationClient,
        IEventBus eventBus)
    {
        _hostGradeRepository = gradeRepository;
        _mapper = mapper;
        _reservationClient = reservationClient;
        _eventBus = eventBus;
    }

    public async Task<DeleteHostGradeResponse> DeleteHostGrade(DeleteHostGradeRequest request)
    {
        var grade = await CheckAndFindGrade(request.gradeId,request.guestId);

        await _hostGradeRepository.DeleteGrade(request.gradeId);
        return new DeleteHostGradeResponse { Success = true };
    }

    public async Task<UpdateHostGradeResponse> UpdateHostGrade(UpdateHostGradeRequest request)
    {
        var grade = await CheckAndFindGrade(request.Id,request.GuestId);

        grade.Number = request.Number;
        await _hostGradeRepository.UpdateGrade(grade);
        return new UpdateHostGradeResponse { Success = true };
    }

    private async Task<HostGrade> CheckAndFindGrade(Guid id,Guid gusetId)
    {
        var grade = await _hostGradeRepository.GetById(id);
        if (grade is null)
        {
            throw new Exception("Grade is not found");
        }

        if (gusetId != grade.GuestId)
        {
            throw new Exception("This is not your host grade, so you cant delete it.");
        }

        return grade;
    }
    

    public async Task<GetGradesByHostResponse> GetGradesByHost(GetGradesByHostRequest request)
    {
        var grades =await  _hostGradeRepository.GetAllByHost(request.HostId);
        if (grades is null)
        {
            throw new Exception("No grades for selected host.");
        }

        return new GetGradesByHostResponse
        {
            HostGrades = grades
        };
    }
    
    public async Task<GetGradesByHostResponse> GetGradesByGuest(GetGradesByGuestRequest request)
    {
        var grades =await  _hostGradeRepository.GetAllByGuest(request.GuestId);
        if (grades is null)
        {
            throw new Exception("No grades for selected guest.");
        }

        return new GetGradesByHostResponse
        {
            HostGrades = grades
        };
    }

    public Result<HostGradeResponse> CreateGradeForHost(HostGradeRequest request)
    {
        var reservations = _reservationClient
            .GetReservationsByGuestAndHostId(request.GuestId,
                request.AccomodationId);
        if (!CheckIfGuestHasStayedInAccommodation(reservations.Reservations))
            return Result.Fail("You cannot grade this host");
        var grade = new HostGrade
        {
            GuestId = request.GuestId,
            Date = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day),
            Number = request.Number,
            HostId = request.HostId
        };
        _hostGradeRepository.CreateGrade(grade);
        var @event = new HostGradeCreatedEvent
        {
            GuestId = request.GuestId,
            GuestEmail = request.GuestEmail,
            Grade = request.Number,
            HostId = request.HostId,
            CreatedAt = DateTime.Now
        };
        _eventBus.PublishAsync(@event);
        return new HostGradeResponse {success = true};
    }
    
    private bool CheckIfGuestHasStayedInAccommodation(RepeatedField<GetReservationDto> reservations)
    {
        return reservations.Any(reservation => reservation.DateRange.To.ToDateTime() < DateTime.Now);
    }
}