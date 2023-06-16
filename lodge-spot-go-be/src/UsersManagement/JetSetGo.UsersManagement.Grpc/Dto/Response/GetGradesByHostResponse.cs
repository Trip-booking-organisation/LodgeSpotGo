using JetSetGo.UsersManagement.Domain.HostGrade.Entities;

namespace JetSetGo.UsersManagement.Grpc.Dto.Response;

public class GetGradesByHostResponse
{
    public List<HostGrade> HostGrades { get; set; } = null!;
}