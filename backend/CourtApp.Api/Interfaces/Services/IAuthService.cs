using CourtApp.Api.DTOs;
using System.Threading.Tasks;

namespace CourtApp.Api.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
}