using Domain.Entities;

namespace Application.Interfaces;

public interface IVenueRepository
{
    Task<Venue?> CreateAsync(Venue entity);
    Task<List<Venue>> GetAllAsync();
    Task<Venue?> GetByIdAsync(int id);
    Task UpdateAsync(Venue entity);
    Task<bool> DeleteAsync(int id);
}