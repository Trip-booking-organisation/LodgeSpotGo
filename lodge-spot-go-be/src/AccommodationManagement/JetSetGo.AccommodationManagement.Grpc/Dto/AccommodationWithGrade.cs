using JetSetGo.AccommodationManagement.Domain.Accommodations;

namespace JetSetGo.AccommodationManagement.Grpc.Dto;

public class AccommodationWithGrade
{
    public Accommodation Accommodation { get; set; }
    public double Grade { get; set; }
}