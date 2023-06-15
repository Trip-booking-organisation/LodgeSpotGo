using JetSetGo.UsersManagement.Domain.HostGrade.Entities;

namespace JetSetGo.UsersManagement.Grpc.Dto;

public class GetGradesByHostResponse
{
    public List<HostGrade> HostGrades { get; set; }
}