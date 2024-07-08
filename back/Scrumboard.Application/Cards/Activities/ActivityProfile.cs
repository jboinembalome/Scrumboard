﻿using AutoMapper;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Application.Cards.Activities;

internal sealed class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        // Read
        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.ActivityType, opt => opt.MapFrom(src => src.ActivityType.ToString()))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.CreatedBy));


        CreateMap<ActivityField, ActivityFieldDto>();
    }
}
