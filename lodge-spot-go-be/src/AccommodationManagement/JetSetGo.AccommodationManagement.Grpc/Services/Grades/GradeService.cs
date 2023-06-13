using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using JetSetGo.AccommodationManagement.Domain.Accommodations.Entities;

namespace JetSetGo.AccommodationManagement.Grpc.Services.Grades;

public class GradeService : GradeApp.GradeAppBase
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IAccommodationRepository _accommodationRepository;

    public GradeService(IGradeRepository gradeRepository, IAccommodationRepository accommodationRepository)
    {
        _gradeRepository = gradeRepository;
        _accommodationRepository = accommodationRepository;
    }

    public override async Task<CreateGradeResponse> CreateGradeForAccommodation(CreateGradeRequest request,
        ServerCallContext context)
    {
        await GetAccommodation(request);
        ValidateRequest(request.Grade.Number);
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
}