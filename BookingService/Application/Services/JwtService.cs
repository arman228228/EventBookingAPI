using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Auth;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;
    
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;

        var key = _configuration["Jwt:SecretKey"];
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }

    public string GenerateToken(int userId, string email, string role)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Email, email)
        };
        
        SigningCredentials signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = signingCredentials
        };
        
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = (JwtSecurityToken)token;
        return tokenHandler.WriteToken(jwtToken);
    }
}