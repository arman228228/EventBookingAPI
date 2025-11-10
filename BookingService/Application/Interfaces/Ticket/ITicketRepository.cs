using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITicketRepository
{
    Task<Ticket?> CreateAsync(Ticket entity);
    Task<List<Ticket>> GetAllAsync();
    Task<Ticket?> GetByIdAsync(int id);
    Task<List<Ticket>> GetByIdsAsync(List<int> ticketIds);
    Task<PagedResult<Ticket>> GetTickets(int page = 1, int pageSize = 10);
    Task UpdateAsync(Ticket entity);
    Task<bool> DeleteAsync(int id);
    Task<int> CountByEventIdAsync(int eventId);
    Task<int> CountByEventIdWithLockAsync(int eventId);
}