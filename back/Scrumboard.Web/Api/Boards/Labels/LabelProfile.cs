using AutoMapper;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Web.Api.Boards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<LabelCreationModel, LabelCreation>();
        CreateMap<LabelEditionModel, LabelEdition>();
        
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
