using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase 
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var registration = await _authService.RegisterAsync(registerDto);
        
        if (registration == null)
        {
            return BadRequest("Registration failed");
        }
        
        return Ok(registration);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var login = await _authService.LoginAsync(loginDto);
        
        if (login == null)
        {
            return BadRequest("Login failed");
        }
        
        return Ok(login);
    }
}