namespace LodgeSpotGo.SearchAndFilter.Grpc.Clients.User;

public interface IUserClient
{
    FiletOutstandingHostResponse IsHostOutstanding(Guid hostId);
}