using Domain.Entities;

namespace Application.Interfaces.Auth;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string role);
}