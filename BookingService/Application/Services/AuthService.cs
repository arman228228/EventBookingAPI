using Application.DTOs;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Interfaces.Auth;
using AutoMapper;
using Domain.Entities;

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
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var userToCreate = new CreateUserDto
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            PasswordHash = passwordHash
        };
        
        var createdUser = await _userService.CreateAsync(userToCreate);

        if (createdUser == null)
        {
            return null;
        }
        
        var token = _jwtService.GenerateToken(createdUser.Id, createdUser.Email, createdUser.Role);
        
        return new AuthResponseDto()
        {
            Token = token
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var userDto = await _userService.GetByEmailAsync(loginDto.Email);
        
        if (userDto == null)
        {
            return null;
        }
        
        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginDto.Password, userDto.PasswordHash);
        
        if (!isPasswordCorrect)
        {
            return null;
        }
        
        var token = _jwtService.GenerateToken(userDto.Id, userDto.Email, userDto.Role);
        
        return new AuthResponseDto()
        {
            Token = token
        };
    }
}