using AutoMapper;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Domain.Boards.Labels;

namespace Scrumboard.Web.Api.Boards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<LabelCreationDto, LabelCreation>();
        CreateMap<LabelEditionDto, LabelEdition>();
        
        // Read
        CreateMap<Label, LabelDto>();
    }
}
