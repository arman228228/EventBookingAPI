using Application.DTOs;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.ResultPatterns;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, ITicketRepository ticketRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<UserCreationResult> CreateAsync(CreateUserDto request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) != null)
        {
            return new UserCreationResult
            {
                Success = false,
                ErrorMessage = "Email already exists"
            };
        }
        
        var userEntity = _mapper.Map<User>(request);
        
        var distinctTicketIds = request.TicketIds.Distinct().ToList();

        if (distinctTicketIds.Count > 0)
        {
            var tickets = await _ticketRepository.GetByIdsAsync(distinctTicketIds);
            if (tickets.Count != distinctTicketIds.Count)
            {
                return new UserCreationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid ticket IDs"
                };
            }

            userEntity.UserTickets = distinctTicketIds
                .Select(ticketId => new UserTicket { TicketId = ticketId })
                .ToList();
        }
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        userEntity.PasswordHash = passwordHash;
        
        userEntity.CreatedAt = DateTime.UtcNow;
        
        await _userRepository.CreateAsync(userEntity);
        
        return new UserCreationResult
        {
            Success = true,
            User = _mapper.Map<UserDto>(userEntity)
        };
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var userEntities = await _userRepository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(userEntities);
    }

    public async Task<UserDto?> GetByIdAsync(int id) 
    {
        var userEntity = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(userEntity);
    }
    
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var userEntity = await _userRepository.GetByEmailAsync(email);
        return _mapper.Map<UserDto>(userEntity);
    }
    
    public async Task<InternalUserDto?> GetInternalByEmailAsync(string email)
    {
        var userEntity = await _userRepository.GetByEmailAsync(email);
        if (userEntity == null) return null;

        return new InternalUserDto
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            Email = userEntity.Email,
            Role = userEntity.Role,
            PasswordHash = userEntity.PasswordHash
        };
    }
    
    public async Task<List<UserDto>> GetByIdsAsync(List<int> usersIds)
    {
        var userEntities = await _userRepository.GetByIdsAsync(usersIds);
        return _mapper.Map<List<UserDto>>(userEntities);
    }

    public async Task<UpdateUserDto> UpdateAsync(int id, UpdateUserDto request)
    {
        var entity = await _userRepository.GetByIdAsync(id);
        if (entity == null)
        {
            return null;
        }

        _mapper.Map(request, entity);
        await _userRepository.UpdateAsync(entity);
        return _mapper.Map<UpdateUserDto>(entity);;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}