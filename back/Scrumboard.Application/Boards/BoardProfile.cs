﻿using AutoMapper;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards;

internal sealed class BoardProfile : Profile
{
    public BoardProfile()
    {
        // Write
        CreateMap<BoardCreation, Board>();
        
        CreateMap<BoardSettingCreation, BoardSetting>();
    }
}
