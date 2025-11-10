namespace Application.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    ITicketRepository _tickets { get; }
    IEventRepository _events { get; }
    IUserRepository _users { get; }
    IVenueRepository _venues { get; }
    
    Task<int> SaveChangesAsync();
    
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}