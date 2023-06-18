using System.Diagnostics;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Application.MessageBroker;
using JetSetGo.AccommodationManagement.Domain.Accommodations;
using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;
using JetSetGo.AccommodationManagement.Grpc.Clients.Reservations;
using JetSetGo.AccommodationManagement.Grpc.Clients.Users;
using LodgeSpotGo.Shared.Events.Grades;
using Status = Grpc.Core.Status;
using StatusCode = Grpc.Core.StatusCode;

namespace JetSetGo.AccommodationManagement.Grpc.Services.Grades;

public class GradeService : GradeApp.GradeAppBase
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IMapper _mapper;
    private readonly IReservationClient _reservationClient;
    private readonly IGetUserClient _userClient;
    private readonly IEventBus _eventBus;
    public const string ServiceName = "GradesService";
    public static readonly ActivitySource ActivitySource = new(ServiceName);


    public GradeService(IGradeRepository gradeRepository, 
        IAccommodationRepository accommodationRepository, 
        IMapper mapper, 
        IReservationClient reservationClient, 
        IGetUserClient userClient, 
        IEventBus eventBus)
    {
        _gradeRepository = gradeRepository;
        _accommodationRepository = accommodationRepository;
        _mapper = mapper;
        _reservationClient = reservationClient;
        _userClient = userClient;
        _eventBus = eventBus;
    }

    public override async Task<CreateGradeResponse> CreateGradeForAccommodation(CreateGradeRequest request,
        ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("GradeNumber", request.Grade.Number.ToString());
        activity?.SetTag("AccommodationId", request.Grade.AccommodationId);
        activity?.SetTag("GuestId", request.Grade.GuestId);
        var accommodation = await GetAccommodation(request);
        ValidateRequest(request.Grade.Number);
        var reservations = _reservationClient
            .GetReservationsByGuestAndHostId(Guid.Parse(request.Grade.GuestId),
                Guid.Parse(request.Grade.AccommodationId));
        if (!CheckIfGuestHasStayedInAccommodation(reservations.Reservations))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You can't grade this accommodation!"));
        var dateNow = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        var grade = new Grade
        {
            Number = request.Grade.Number,
            AccommodationId = Guid.Parse(request.Grade.AccommodationId),
            GuestId = Guid.Parse(request.Grade.GuestId),
            Date = dateNow
        };
        await _gradeRepository.CreateGrade(grade);
        var @event = new AccommodationGradeCreated
        {
            Grade = request.Grade.Number,
            AccommodationId = accommodation.Id,
            GuestId = Guid.Parse(request.Grade.GuestId),
            GuestEmail = request.Grade.GuestEmail,
            CreatedAt = DateTime.Now,
            HostId = accommodation.HostId,
            AccommodationName = accommodation.Name
        };
        await _eventBus.PublishAsync(@event);
        activity?.Stop();
        return new CreateGradeResponse { Success = true };
        // Add attributes or events to the span as needed
        
    }

    private bool CheckIfGuestHasStayedInAccommodation(IEnumerable<GetReservationDto> reservations)
    {
        return reservations.Any(reservation => reservation.DateRange.To.ToDateTime() < DateTime.Now);
    }

    private static void ValidateRequest(int gradeNumber)
    {
        if (gradeNumber is < 1 or > 5)
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                "Grade number should be greater between 1 and 5!"));
    }

    private async Task<Accommodation> GetAccommodation(CreateGradeRequest request)
    {
        var accommodation = await _accommodationRepository
            .GetAsync(Guid.Parse(request.Grade.AccommodationId));
        if (accommodation is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Accommodation doesn't exists!"));
        return accommodation;
    }

    public override async Task<UpdateGradeResponse> UpdateGradeForAccommodation(UpdateGradeRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("GradeNumber", request.Grade.Number.ToString());
        activity?.SetTag("GradeId", request.Grade.Id);
        var grade = await GetGradeById(request);
        ValidateRequest(request.Grade.Number);
        grade.Number = request.Grade.Number;
        await _gradeRepository.UpdateGrade(grade);
        activity?.Stop();
        return new UpdateGradeResponse { Success = true };
    }

    private async Task<Grade> GetGradeById(UpdateGradeRequest request)
    {
        var grade = await _gradeRepository.GetById(Guid.Parse(request.Grade.Id));
        if (grade is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Grade doesn't exists!"));
        return grade;
    }

    public override async Task<GetAllGradesResponse> GetAllGrades(GetAllGradesRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        var response = new GetAllGradesResponse();
        var grades = await _gradeRepository.GetAllAsync();
        var responseList = grades.Select(grade => _mapper.Map<GradeDto>(grade)).ToList();

        responseList.ForEach(dto => response.Grades.Add(dto));
        activity?.Stop();
        return response;
    }

    public override async Task<DeleteGradeResponse> DeleteGrade(DeleteGradeRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("GradeId", request.Id);
        var grade = await _gradeRepository.GetById(Guid.Parse(request.Id));
        if (grade is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Grade not found!"));
        await _gradeRepository.DeleteGrade(grade.Id);
        activity?.Stop();
        return new DeleteGradeResponse { Success = true };
    }

    public override async Task<GetGradesByGuestResponse> GetGradesByGuest(GetGradesByGuestRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("GradeId", request.GuestId);
        var grades = await _gradeRepository.GetAllByGuest(Guid.Parse(request.GuestId));
        var response = new GetGradesByGuestResponse();

        /*async void Action(Grade x)
        {
            var accommodation = await _accommodationRepository.GetAsync(x.AccommodationId);
            var dto = new GradeByGuest
            {
                Number = x.Number,
                Id = x.Id.ToString(), 
                Accommodation = _mapper.Map<AccommodationDto>(accommodation)
            };
            response.Grades.Add(dto);
        }*/

        grades.ForEach(x =>
        {
            var accommodation = _accommodationRepository.Get(x.AccommodationId);
            var dto = new GradeByGuest
            {
                Number = x.Number,
                Id = x.Id.ToString(), 
                Accommodation = _mapper.Map<AccommodationDto>(accommodation)
            };
            response.Grades.Add(dto);
        });
        activity?.Stop();
        return response;
    }

    public override async Task<GetGradesByAccommodationResponse> GetGradesByAccommodation(GetGradesByAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("AccommodationId", request.AccommodationId);
        var grades = await _gradeRepository.GetByAccommodation(Guid.Parse(request.AccommodationId));
        var response = new GetGradesByAccommodationResponse();
        var grade = 0;
        grades.ForEach(x =>
        {
            var user = _userClient.GetUserInfo(x.GuestId);
            var date = new DateTimeOffset(x.Date.ToDateTime(TimeOnly.MinValue));
            var grader = new Grader
            {
                Id = user.Id.ToString(),
                Mail = user.Email,
                Name = user.Name,
                Surname = user.LastName
            };
            var res = new AccommodationGradeResponse()
            {
                Id = x.Id.ToString(),
                Number = x.Number,
                AccommodationId = x.AccommodationId.ToString(),
                Guest = grader,
                Date = Timestamp.FromDateTimeOffset(date)
            };
            grade += x.Number;
            response.AccommodationGrade.Add(res);
        });
        var averageGrade = 0;
        if(grades.Count !=0)
            averageGrade = grade / grades.Count;
        response.AverageGrade = averageGrade;
        activity?.Stop();
        return response;
    }
}