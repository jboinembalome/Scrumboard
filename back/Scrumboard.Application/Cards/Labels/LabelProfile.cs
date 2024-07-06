﻿using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Cards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<LabelDto, Label>()
            .EqualityComparison((src, dest) => src.Id == dest.Id)
            .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour));

        // Read
        CreateMap<Label, LabelDto>();
    }
}
