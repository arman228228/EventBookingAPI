using Application.DTOs;

namespace Application.Interfaces;

public interface IVenueService
{
    Task<VenueDto?> CreateAsync(CreateVenueDto request);
    Task<List<VenueDto>> GetAllAsync();
    Task<VenueDto?> GetByIdAsync(int id);
    Task<UpdateVenueDto> UpdateAsync(int id, UpdateVenueDto request);
    Task<bool> DeleteAsync(int id);
}