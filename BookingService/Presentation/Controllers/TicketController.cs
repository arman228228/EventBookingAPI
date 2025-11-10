using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IEventService _eventService;
    private readonly IUserService _userService;
    
    public TicketController(ITicketService ticketService, IEventService eventService, IUserService userService)
    {
        _ticketService = ticketService;
        _eventService = eventService;
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<ActionResult<TicketDto>> CreateAsync(CreateTicketDto request)
    {
        var users = await _userService.GetByIdsAsync(request.UserIds);
        
        if (users.Count != request.UserIds.Count)
        {
            return NotFound($"Not all users found");
        }
        
        var result = await _ticketService.CreateAsync(request);

        if (!result.Success)
        {
            return NotFound(result.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Ticket.Id }, result.Ticket);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TicketDto>>> GetAllAsync()
    {
        var tickets = await _ticketService.GetAllAsync();

        if (!tickets.Any())
        {
            return NotFound("No tickets found");
        }
        
        return Ok(tickets);
    }
    
    [HttpGet("{id}")]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<TicketDto>> GetByIdAsync(int id)
    {
        var ticketEntity = await _ticketService.GetByIdAsync(id);
        
        if(ticketEntity == null)
        {
            return NotFound($"Ticket {id} not found");
        }
        
        return Ok(ticketEntity);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<TicketDto>>> GetTickets(int page = 1, int pageSize = 10)
    {
        var tickets = await _ticketService.GetTickets(page, pageSize);
      
        return Ok(tickets);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<UpdateTicketDto?>> UpdateAsync(int id, UpdateTicketDto request)
    {
        var ticketDto = await _ticketService.UpdateAsync(id, request);
        
        if(ticketDto == null)
        {
            return NotFound($"Ticket {id} not found");
        }
        
        return Ok(ticketDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var success = await _ticketService.DeleteAsync(id);
        
        if(!success)
        {
            return NotFound($"Ticket {id} not found");
        }
        
        return NoContent();
    }
}