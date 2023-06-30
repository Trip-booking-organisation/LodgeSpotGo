namespace JetSetGo.UsersManagement.Grpc.Client.Accommodations;

public interface IAccommodationClient
{
    GetAccommodationHostResponse GetAccommodationByHost(Guid hostId);
}