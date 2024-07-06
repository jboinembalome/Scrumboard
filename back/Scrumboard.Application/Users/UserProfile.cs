﻿using AutoMapper;
using Scrumboard.Application.Users.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Users;

internal sealed class UserProfile : Profile
{
    public UserProfile()
    {
        // Read
        CreateMap<IUser, UserDto>()
            .ForMember(dest => dest.HasAvatar, opt => opt.MapFrom(src => src.Avatar.Any()));
        
        CreateMap<string, UserDto>()
            .ConstructUsing(userId => new UserDto
            {
                Id = userId
            });
        
        // Write
        CreateMap<UserDto, string>()
            .ConstructUsing(x => x.Id);
    }
}
