using CourtApp.Api.Models;

using CourtApp.Api.Interfaces.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }
}