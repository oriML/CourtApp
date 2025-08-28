using CourtApp.Api.Models;

namespace CourtApp.Api.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user);
}