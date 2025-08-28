using CourtApp.Api.DTOs;
using CourtApp.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourtApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var response = await _authService.LoginAsync(loginRequestDto);
        if (response == null)
        {
            return Unauthorized("Invalid credentials");
        }
        return Ok(response);
    }
}