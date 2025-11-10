using Application.DTOs;
using Application.DTOs.Auth;
using Application.ResultPatterns;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserCreationResult> CreateAsync(CreateUserDto request);
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<InternalUserDto?> GetInternalByEmailAsync(string email);
    Task<List<UserDto>> GetByIdsAsync(List<int> users);
    Task<UpdateUserDto> UpdateAsync(int id, UpdateUserDto request);
    Task<bool> DeleteAsync(int id);
}