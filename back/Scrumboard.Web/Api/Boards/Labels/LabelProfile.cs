using AutoMapper;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Web.Api.Boards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<LabelCreationDto, LabelCreation>();
        CreateMap<LabelEditionDto, LabelEdition>();
        
        CreateMap<LabelDto, int>()
            .ConstructUsing(labelDto => labelDto.Id);
        
        // Read
        CreateMap<Label, LabelDto>();
        
        CreateMap<int, LabelDto>()
            .ConstructUsing(labelId => new LabelDto
            {
                Id = labelId
            });
    }
}
