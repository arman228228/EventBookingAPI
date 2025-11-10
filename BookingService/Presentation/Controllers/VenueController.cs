using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class VenueController : ControllerBase
{
    private readonly IVenueService _venueService;

    public VenueController(IVenueService venueService)
    {
        _venueService = venueService;
    }
    
    [HttpPost]
    public async Task<ActionResult<VenueDto>> CreateAsync(CreateVenueDto request)
    {
        var venueDto = await _venueService.CreateAsync(request);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = venueDto.Id }, venueDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<VenueDto>>> GetAllAsync()
    {
        var venues = await _venueService.GetAllAsync();

        if (!venues.Any())
        {
            return NotFound("No venues found");
        }
        
        return Ok(venues);
    }
    
    [HttpGet("{id}")]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<Venue>> GetByIdAsync(int id)
    {
        var venueEntity = await _venueService.GetByIdAsync(id);
        
        if(venueEntity == null)
        {
            return NotFound($"Venue {id} not found");
        }
        
        return Ok(venueEntity);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<UpdateVenueDto?>> UpdateAsync(int id, UpdateVenueDto request)
    {
        var venueDto = await _venueService.UpdateAsync(id, request);
        
        if(venueDto == null)
        {
            return NotFound($"Venue {id} not found");
        }
        
        return Ok(venueDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var success = await _venueService.DeleteAsync(id);
        
        if(!success)
        {
            return NotFound($"Venue {id} not found");
        }
        
        return NoContent();
    }
}