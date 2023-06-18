using System.Diagnostics;
using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;
using LodgeSpotGo.AccommodationManagement.Grpc;

namespace JetSetGo.AccommodationManagement.Grpc.Services.Grades;

public class FilterGrades : FilterGradeApp.FilterGradeAppBase
{
    private readonly IGradeRepository _gradeRepository;
    public const string ServiceName = "FilterGradesService";
    public static readonly ActivitySource ActivitySource = new(ServiceName);
    public FilterGrades(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public override async Task<GetAverageGradeByAccommodationResponse> GetAverageGrade(GetAverageGradeByAccommodationRequest request, ServerCallContext context)
    {
        var activity = ActivitySource.StartActivity();
        activity?.SetTag("AccommodationId", request.AccommodationId);
        var grades = await _gradeRepository.GetByAccommodation(Guid.Parse(request.AccommodationId));
        var gradeNumber = 0;
        grades.ForEach(x =>
        {
            gradeNumber += x.Number;
        });
        var averageGrade = gradeNumber / grades.Count;
        activity?.Stop();
        return new GetAverageGradeByAccommodationResponse
        {
            AverageGradeNumber = averageGrade
        };
    }
}