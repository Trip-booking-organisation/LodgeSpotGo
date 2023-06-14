using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using LodgeSpotGo.AccommodationManagement.Grpc;

namespace JetSetGo.AccommodationManagement.Grpc.Services.Grades;

public class FilterGrades : FilterGradeApp.FilterGradeAppBase
{
    private readonly IGradeRepository _gradeRepository;

    public FilterGrades(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public override async Task<GetAverageGradeByAccommodationResponse> GetAverageGrade(GetAverageGradeByAccommodationRequest request, ServerCallContext context)
    {
        var grades = await _gradeRepository.GetByAccommodation(Guid.Parse(request.AccommodationId));
        var gradeNumber = 0;
        grades.ForEach(x =>
        {
            gradeNumber += x.Number;
        });
        var averageGrade = gradeNumber / grades.Count;
        return new GetAverageGradeByAccommodationResponse
        {
            AverageGradeNumber = averageGrade
        };
    }
}