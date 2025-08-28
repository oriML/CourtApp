using Xunit;
using Moq;
using CourtApp.Api.Interfaces.Services;
using CourtApp.Api.Interfaces.Repositories;
using CourtApp.Api.DTOs;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace CourtApp.Api.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _service = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrowNotImplementedException()
    {
        // Arrange
        var userDto = new UserDto();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _service.CreateUserAsync(userDto));
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotImplementedException()
    {
        // Arrange
        var id = 1; // Assuming int for now based on UserService.cs

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _service.GetUserByIdAsync(id));
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldThrowNotImplementedException()
    {
        // Arrange
        var username = "testuser";

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _service.GetUserByUsernameAsync(username));
    }
}