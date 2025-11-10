using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }
    
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateAsync(CreateEventDto request)
    {
        var eventDto = await _eventService.CreateAsync(request);

        if (eventDto == null)
        {
            return NotFound("Venue not found");
        }

        return CreatedAtAction(nameof(GetByIdAsync), new { id = eventDto.Id }, eventDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetAllAsync()
    {
        var events = await _eventService.GetAllAsync();

        if (!events.Any())
        {
            return NotFound("No events found");
        }
        
        return Ok(events);
    }
    
    [HttpGet("{id}")]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<EventDto>> GetByIdAsync(int id)
    {
        var eventDto = await _eventService.GetByIdAsync(id);
        
        if(eventDto == null)
        {
            return NotFound($"Event {id} not found");
        }
        
        return Ok(eventDto);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<UpdateEventDto?>> UpdateAsync(int id, UpdateEventDto request)
    {
        var eventDto = await _eventService.UpdateAsync(id, request);
        
        if(eventDto == null)
        {
            return NotFound($"Event {id} not found");
        }
        
        return Ok(eventDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var success = await _eventService.DeleteAsync(id);
        
        if(!success)
        {
            return NotFound($"Event {id} not found");
        }
        
        return NoContent();
    }
}