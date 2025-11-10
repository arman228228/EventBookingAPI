using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateAsync(CreateUserDto request)
    {
        var userCreationResult = await _userService.CreateAsync(request);

        if (userCreationResult.Success == false)
        {
            return BadRequest(userCreationResult.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetByIdAsync), new { id = userCreationResult.User.Id }, userCreationResult.User);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllAsync()
    {
        var users = await _userService.GetAllAsync();

        if (!users.Any())
        {
            return NotFound("No users found");
        }
        
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    [ActionName(nameof(GetByIdAsync))]
    public async Task<ActionResult<UserDto>> GetByIdAsync(int id)
    {
        var userEntity = await _userService.GetByIdAsync(id);
        
        if(userEntity == null)
        {
            return NotFound($"User {id} not found");
        }
        
        return Ok(userEntity);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<UpdateUserDto?>> UpdateAsync(int id, UpdateUserDto request)
    {
        var userDto = await _userService.UpdateAsync(id, request);
        
        if(userDto == null)
        {
            return NotFound($"User {id} not found");
        }
        
        return Ok(userDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var success = await _userService.DeleteAsync(id);
        
        if(!success)
        {
            return NotFound($"User {id} not found");
        }
        
        return NoContent();
    }
}