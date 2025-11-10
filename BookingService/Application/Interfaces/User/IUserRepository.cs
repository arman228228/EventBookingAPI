using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<User?> CreateAsync(User entity);
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetByIdsAsync(List<int> users);
    Task UpdateAsync(User entity);
    Task<bool> DeleteAsync(int id);
}