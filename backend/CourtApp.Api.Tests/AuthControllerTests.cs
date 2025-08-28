using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using CourtApp.Api.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace CourtApp.Api.Tests;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ReturnsOkStatusCode_WithValidCredentials()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Username = "admin", Password = "password" };
        var json = JsonConvert.SerializeObject(loginDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/Auth/login", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(responseString);
        Assert.NotNull(loginResponse);
        Assert.NotNull(loginResponse.Token);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorizedStatusCode_WithInvalidCredentials()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Username = "admin", Password = "wrongpassword" };
        var json = JsonConvert.SerializeObject(loginDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/Auth/login", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorizedStatusCode_WhenUserNotFound()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Username = "nonexistent", Password = "password123" };
        var json = JsonConvert.SerializeObject(loginDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/Auth/login", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
