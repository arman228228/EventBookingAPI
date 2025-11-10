namespace Application.DTOs.Auth;

public class InternalUserDto : UserDto
{
    public string PasswordHash { get; set; } = null;
}