using System.Data;
using Application.Interfaces;
using Application.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IDbContextTransaction? _transaction;
    
    public ITicketRepository _tickets { get; set; }
    public IEventRepository _events { get; set; }
    public IUserRepository _users { get; set; }
    public IVenueRepository _venues { get; set; }
    
    public UnitOfWork(AppDbContext dbContext, 
        ITicketRepository tickets,
        IEventRepository events,
        IUserRepository users,
        IVenueRepository venues
    )
    {
        _dbContext = dbContext;
        
        _tickets = tickets;
        _events = events;
        _users = users;
        _venues = venues;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    
    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }
    
    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }
    
    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }
    
    public void Dispose()
    {
        _transaction?.Dispose();
        _dbContext?.Dispose();
    }
}