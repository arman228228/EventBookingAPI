using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;
    
    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> CreateAsync(User entity)
    {
        await _dbContext.Users.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<List<User>> GetByIdsAsync(List<int> userIds)
    {
        return await _dbContext.Users.Where(t => userIds.Contains(t.Id)).ToListAsync();
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }
    
    public async Task UpdateAsync(User entity)
    {
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var existingUser = await _dbContext.Users.FindAsync(id);
        if (existingUser == null)
        {
            return false;
        }
        
        _dbContext.Users.Remove(existingUser);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}