using Application.DTOs;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Interfaces.Auth;
using AutoMapper;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    
    public AuthService(IUserService userService, IJwtService jwtService, IMapper mapper)
    {
        _userService = userService;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userService.GetByEmailAsync(registerDto.Email) != null)
        {
            return null;
        }
        
        var userToCreate = new CreateUserDto
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Password = registerDto.Password
        };
        
        var createdUser = await _userService.CreateAsync(userToCreate);
        
        if (createdUser.Success == false)
        {
            return null;
        }
        
        var token = _jwtService.GenerateToken(createdUser.User.Id, createdUser.User.Email, createdUser.User.Role);
        
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtService.TokenExpirationMinutes);
        
        return new AuthResponseDto()
        {
            Token = token,
            ExpiresAt = expiresAt
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userService.GetInternalByEmailAsync(loginDto.Email);
        if (user == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return null;
        }
        
        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);
        
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtService.TokenExpirationMinutes);
        
        return new AuthResponseDto()
        {
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}