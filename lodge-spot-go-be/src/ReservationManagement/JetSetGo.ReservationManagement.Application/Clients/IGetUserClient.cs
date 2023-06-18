using JetSetGo.ReservationManagement.Application.Dto.Response;

namespace JetSetGo.ReservationManagement.Application.Clients;

public interface IGetUserClient
{
    public UserResponse GetUserInfo(Guid id);
}