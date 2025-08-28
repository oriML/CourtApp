using CourtApp.Api.DTOs;

namespace CourtApp.Api.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<UserDto> CreateUserAsync(UserDto userDto);
}