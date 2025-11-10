using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> CreateAsync(CreateUserDto request)
    {
        var userEntity = _mapper.Map<User>(request);
        userEntity.CreatedAt = DateTime.UtcNow;
        
        await _userRepository.CreateAsync(userEntity);
        
        return _mapper.Map<UserDto>(userEntity);
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