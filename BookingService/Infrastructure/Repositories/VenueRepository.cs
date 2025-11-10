using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly AppDbContext _dbContext;
    
    public VenueRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Venue?> CreateAsync(Venue entity)
    {
        await _dbContext.Venues.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }
    
    public async Task<Venue?> GetByIdAsync(int id)
    {
        return await _dbContext.Venues.FindAsync(id);
    }
    
    public async Task<List<Venue>> GetAllAsync()
    {
        return await _dbContext.Venues.ToListAsync();
    }
    
    public async Task UpdateAsync(Venue entity)
    {
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var existingVenue = await _dbContext.Venues.FindAsync(id);
        if (existingVenue == null)
        {
            return false;
        }
        
        _dbContext.Venues.Remove(existingVenue);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}