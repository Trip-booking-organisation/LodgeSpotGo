namespace JetSetGo.AccommodationManagement.Grpc.Clients.Users;

public interface IUserClient
{
    GetUserResponse GetUserById(Guid userId);
}