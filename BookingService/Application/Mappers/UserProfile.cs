using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
        
        CreateMap<CreateUserDto, User>();
        CreateMap<User, CreateUserDto>();
        
        CreateMap<UpdateUserDto, User>();
        CreateMap<User, UpdateUserDto>();
    }
}