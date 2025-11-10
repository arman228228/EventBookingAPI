using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IVenueService _venueService;
    private readonly IMapper _mapper;
    
    public EventService(IEventRepository eventRepository, IVenueService venueService, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _venueService = venueService;
        _mapper = mapper;
    }

    public async Task<EventDto?> CreateAsync(CreateEventDto request)
    {
        if (await _venueService.GetByIdAsync(request.VenueId) == null)
        {
            return null;
        }
        
        var eventEntity = _mapper.Map<Event>(request);
        eventEntity.CreatedAt = DateTime.UtcNow;
        
        await _eventRepository.CreateAsync(eventEntity);
        
        return _mapper.Map<EventDto>(eventEntity);
    }

    public async Task<List<EventDto>> GetAllAsync()
    {
        var events = await _eventRepository.GetAllAsync();
        return _mapper.Map<List<EventDto>>(events);
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(id);
        return _mapper.Map<EventDto>(eventEntity);
    }

    public async Task<UpdateEventDto> UpdateAsync(int id, UpdateEventDto request)
    {
        var entity = await _eventRepository.GetByIdAsync(id);
        if (entity == null)
        {
            return null;
        }

        _mapper.Map(request, entity);
        
        await _eventRepository.UpdateAsync(entity);
        return _mapper.Map<UpdateEventDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id) => await _eventRepository.DeleteAsync(id);
}