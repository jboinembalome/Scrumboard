﻿using AutoMapper;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<LabelCreation, LabelDao>();
        CreateMap<LabelEdition, LabelDao>();

        // Read
        CreateMap<LabelDao, Label>();
    }
}
