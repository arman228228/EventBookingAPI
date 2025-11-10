using Application.DTOs;

namespace Application.Interfaces;

public interface IEventService
{
    Task<EventDto?> CreateAsync(CreateEventDto request);
    Task<List<EventDto>> GetAllAsync();
    Task<EventDto?> GetByIdAsync(int id);
    Task<UpdateEventDto> UpdateAsync(int id, UpdateEventDto request);
    Task<bool> DeleteAsync(int id);
}