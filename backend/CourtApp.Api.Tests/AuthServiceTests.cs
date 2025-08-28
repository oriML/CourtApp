using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using CourtApp.Api.Interfaces.Services;
using CourtApp.Api.DTOs;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Linq; // Added for .First()

namespace CourtApp.Api.Tests;

public class AuthServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup configuration for JWT
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("thisisalongkeyforjwttokengenerationthatisatleast256bits");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("testissuer");
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("testaudience");

        _service = new AuthService(_mockConfiguration.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnLoginResponseDto_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Username = "admin", Password = "password" };

        // Act
        var result = await _service.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);

        // Optionally, validate the token content
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(result.Token);

        Assert.Contains(token.Claims, c => c.Type == "sub" && c.Value == "admin");
        Assert.Contains(token.Claims, c => c.Type == "email" && c.Value == "admin");
        Assert.Contains(token.Claims, c => c.Type == "role" && c.Value == "Admin");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Username = "nonexistent", Password = "password123" };

        // Act
        var result = await _service.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenInvalidPassword()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Username = "admin", Password = "wrongpassword" };

        // Act
        var result = await _service.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }
}