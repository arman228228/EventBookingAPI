using Domain.Entities;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<Event?> CreateAsync(Event entity);
    Task<List<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(int id);
    Task UpdateAsync(Event entity);
    Task<bool> DeleteAsync(int id);
}