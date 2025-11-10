using Application.DTOs.Auth;

namespace Application.Interfaces.Auth;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerRequest);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginRequest);
}