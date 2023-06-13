using AutoMapper;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;
using JetSetGo.AccommodationManagement.Grpc.Clients;

namespace JetSetGo.AccommodationManagement.Grpc.Services.Grades;

public class GradeService : GradeApp.GradeAppBase
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IMapper _mapper;
    private readonly IReservationClient _reservationClient;


    public GradeService(IGradeRepository gradeRepository, IAccommodationRepository accommodationRepository, IMapper mapper, IReservationClient reservationClient)
    {
        _gradeRepository = gradeRepository;
        _accommodationRepository = accommodationRepository;
        _mapper = mapper;
        _reservationClient = reservationClient;
    }

    public override async Task<CreateGradeResponse> CreateGradeForAccommodation(CreateGradeRequest request,
        ServerCallContext context)
    {
        await GetAccommodation(request);
        ValidateRequest(request.Grade.Number);
        var reservations = _reservationClient
            .GetReservationsByGuestAndHostId(Guid.Parse(request.Grade.GuestId),
                Guid.Parse(request.Grade.AccommodationId));
        if (!CheckIfGuestHasStayedInAccommodation(reservations.Reservations))
            throw new RpcException(new Status(StatusCode.Cancelled, "You can't grade this accommodation!"));
        var grade = new Grade
        {
            Number = request.Grade.Number,
            AccommodationId = Guid.Parse(request.Grade.AccommodationId),
            GuestId = Guid.Parse(request.Grade.GuestId),
            Date = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day)
        };
        await _gradeRepository.CreateGrade(grade);
        return new CreateGradeResponse { Success = true };
    }

    private bool CheckIfGuestHasStayedInAccommodation(IEnumerable<GetReservationDto> reservations)
    {
        return reservations.Any(reservation => reservation.DateRange.To.ToDateTime() < DateTime.Now);
    }

    private static void ValidateRequest(int gradeNumber)
    {
        if (gradeNumber is < 1 or > 5)
            throw new RpcException(new Status(StatusCode.Cancelled,
                "Grade number should be greater between 1 and 5!"));
    }

    private async Task GetAccommodation(CreateGradeRequest request)
    {
        var accommodation = await _accommodationRepository
            .GetAsync(Guid.Parse(request.Grade.AccommodationId));
        if (accommodation is null)
            throw new RpcException(new Status(StatusCode.Cancelled, "Accommodation doesn't exists!"));
    }

    public override async Task<UpdateGradeResponse> UpdateGradeForAccommodation(UpdateGradeRequest request, ServerCallContext context)
    {
       var grade =  await GetGradeById(request);
       ValidateRequest(request.Grade.Number);
       grade.Number = request.Grade.Number;
       await _gradeRepository.UpdateGrade(grade);
       return new UpdateGradeResponse { Success = true };
    }

    private async Task<Grade> GetGradeById(UpdateGradeRequest request)
    {
        var grade = await _gradeRepository.GetById(Guid.Parse(request.Grade.Id));
        if (grade is null)
            throw new RpcException(new Status(StatusCode.Cancelled, "Grade doesn't exists!"));
        return grade;
    }

    public override async Task<GetAllGradesResponse> GetAllGrades(GetAllGradesRequest request, ServerCallContext context)
    { 
       var response = new GetAllGradesResponse();
       var grades =  await _gradeRepository.GetAllAsync();
       var responseList = grades.Select(grade => _mapper.Map<GradeDto>(grade)).ToList();
        
       responseList.ForEach(dto => response.Grades.Add(dto));
       return response;
    }

    public override async Task<DeleteGradeResponse> DeleteGrade(DeleteGradeRequest request, ServerCallContext context)
    {
        var grade = await _gradeRepository.GetById(Guid.Parse(request.Id));
        if (grade is null)
            throw new RpcException(new Status(StatusCode.Cancelled, "Grade not found!"));
        await _gradeRepository.DeleteGrade(grade.Id);
        return new DeleteGradeResponse { Success = true };
    }

    public override async Task<GetGradesByGuestResponse> GetGradesByGuest(GetGradesByGuestRequest request, ServerCallContext context)
    {
        var grades = await _gradeRepository.GetAllByGuest(Guid.Parse(request.GuestId));
        var response = new GetGradesByGuestResponse();

        async void Action(Grade x)
        {
            var accommodation = await _accommodationRepository.GetAsync(x.AccommodationId);
            var dto = new GradeByGuest
            {
                Number = x.Number,
                Id = x.Id.ToString(), 
                Accommodation = _mapper.Map<AccommodationDto>(accommodation)
            };
            response.Grades.Add(dto);
        }

        grades.ForEach(Action);
        return response;
    }

    /*public override async Task<GetGradesByAccommodationResponse> GetGradesByAccommodation(GetGradesByAccommodationRequest request, ServerCallContext context)
    {
        var grades = await _gradeRepository.GetByAccommodation(Guid.Parse(request.AccommodationId));
        var responseList = new List<AccommodationGradeResponse>();
        grades.ForEach(x =>
        {
            var res = new AccommodationGradeResponse()
            {
                Id = x.Id.ToString(),
                Number = x.Number,
                AccommodationId = x.AccommodationId.ToString(),
                Guest = 
                
            }
        });
        var response = new GetGradesByAccommodationResponse();
        
    }*/
}