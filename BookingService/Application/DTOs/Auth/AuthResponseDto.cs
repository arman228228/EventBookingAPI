namespace Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
}