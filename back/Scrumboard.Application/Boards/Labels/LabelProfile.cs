using AutoMapper;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Domain.Boards.Labels;

namespace Scrumboard.Application.Boards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<LabelCreation, Label>();
        CreateMap<LabelEdition, Label>();
    }
}
