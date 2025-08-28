using CourtApp.Api.DTOs;
using CourtApp.Api.Interfaces.Repositories;

using CourtApp.Api.Interfaces.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserDto> CreateUserAsync(UserDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }
}