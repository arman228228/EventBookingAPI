using Application.DTOs;

namespace Application.ResultPatterns;

public class UserCreationResult
{
    public UserDto? User { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}