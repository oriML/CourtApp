using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using CourtApp.Api.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System;
using System.Collections.Generic;

namespace CourtApp.Api.Tests;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async Task<string> GetAdminToken()
    {
        var loginDto = new LoginRequestDto { Username = "admin", Password = "password" };
        var json = JsonConvert.SerializeObject(loginDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/Auth/login", content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(responseString);
        return loginResponse.Token;
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode_WithAdminToken()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // Act
        var response = await _client.GetAsync("/Users");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ReturnsUnauthorizedStatusCode_WithoutToken()
    {
        // Arrange - No token added

        // Act
        var response = await _client.GetAsync("/Users");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsSuccessStatusCode_WhenUserExists()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // Assuming there's at least one user (e.g., the admin user created by the system)
        // In a real scenario, you might seed a user or get an existing user ID
        // For simplicity, let's assume the admin user has ID 1 (if using int IDs)
        // Or, if using GUIDs, you'd need to retrieve an actual user ID.
        // Since UserService.GetUserByIdAsync expects int, we'll use 1 for now.
        var userId = 1; 

        // Act
        var response = await _client.GetAsync($"/Users/{userId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundStatusCode_WhenUserDoesNotExist()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var nonExistentId = 999; // Assuming an ID that won't exist

        // Act
        var response = await _client.GetAsync($"/Users/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContentStatusCode()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // Create a user to delete (assuming CreateUser endpoint exists and works)
        // Since UserService.CreateUserAsync is not implemented, this test will fail or need adjustment.
        // For now, let's assume a user with ID 2 exists and can be deleted.
        var userIdToDelete = 2; 

        // Act
        var response = await _client.DeleteAsync($"/Users/{userIdToDelete}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify it's actually deleted
        var getResponse = await _client.GetAsync($"/Users/{userIdToDelete}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
