using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _dbContext;
    
    public EventRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Event?> CreateAsync(Event entity)
    {
        await _dbContext.Events.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }
    
    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _dbContext.Events.FindAsync(id);
    }
    
    public async Task<List<Event>> GetAllAsync()
    {
        return await _dbContext.Events.ToListAsync();
    }
    
    public async Task UpdateAsync(Event entity)
    {
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var existingEvent = await _dbContext.Events.FindAsync(id);
        if (existingEvent == null)
        {
            return false;
        }
        
        _dbContext.Events.Remove(existingEvent);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}