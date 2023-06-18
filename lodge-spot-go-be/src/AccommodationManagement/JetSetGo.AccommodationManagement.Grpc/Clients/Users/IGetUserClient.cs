using JetSetGo.AccommodationManagement.Application.Dto.Response;

namespace JetSetGo.AccommodationManagement.Grpc.Clients.Users;

public interface IGetUserClient
{
    public UserResponse GetUserInfo(Guid id);
}