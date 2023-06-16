﻿using Grpc.Core;
using LodgeSpotGo.RecommodationSystem.Core.Model;
using LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

namespace JetSetGo.RecommodationSystem.Grpc.Services;

public class UserService : UserApp.UserAppBase
{
    private readonly IRecommodationRepository _recommodationRepository;

    public UserService(IRecommodationRepository recommodationRepository)
    {
        _recommodationRepository = recommodationRepository;
    }
    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        Guest guest = new Guest
        {
            Gmail = request.User.Gmail,
            Name = request.User.Name,
            Surname = request.User.Surname
        };
        var response = await _recommodationRepository.CreateGuest(guest);
        CreateUserResponse bla = new CreateUserResponse
        {
            User = new User
            {
                Gmail = response.Gmail,
                Name = response.Name,
                Surname = response.Surname
            }
        };
        return bla;
    }
}