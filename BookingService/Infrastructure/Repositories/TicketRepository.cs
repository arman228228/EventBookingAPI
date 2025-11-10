using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _dbContext;
    
    public TicketRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Ticket?> CreateAsync(Ticket entity)
    {
        await _dbContext.Tickets.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }
    
    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _dbContext.Tickets
            .Include(t => t.UserTickets)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Ticket>> GetByIdsAsync(List<int> ticketIds)
    {
        return await _dbContext.Tickets
            .Where(t => ticketIds.Contains(t.Id))
            .Include(t => t.UserTickets)
            .ToListAsync();
    }
    
    public async Task<List<Ticket>> GetAllAsync()
    {
        return await _dbContext.Tickets
            .Include(t => t.UserTickets)
            .ToListAsync();
    }
    
    public async Task<PagedResult<Ticket>> GetTickets(int page = 1, int pageSize = 10)
    {
        var query = _dbContext.Tickets.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(q => q.Id)
            .Skip((page - 1) * pageSize)
            .Include(t => t.UserTickets)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedResult<Ticket>
        {
            TotalCount = totalCount,
            Items = items,
            Page = page,
            PageSize = pageSize
        };
        
        return result;
    }
    
    public async Task UpdateAsync(Ticket entity)
    {
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var existingTicket = await _dbContext.Tickets.FindAsync(id);
        if (existingTicket == null)
        {
            return false;
        }
        
        _dbContext.Tickets.Remove(existingTicket);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<int> CountByEventIdAsync(int eventId)
    {
        return await _dbContext.Tickets
            .Where(t => t.EventId == eventId)
            .CountAsync();
    }
    
    public async Task<int> CountByEventIdWithLockAsync(int eventId)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            @"LOCK TABLE ""Tickets"" IN SHARE ROW EXCLUSIVE MODE");

        return await _dbContext.Tickets
            .Where(t => t.EventId == eventId)
            .CountAsync();
    }
}