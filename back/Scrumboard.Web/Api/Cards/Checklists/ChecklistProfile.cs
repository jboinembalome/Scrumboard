using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Checklists;

namespace Scrumboard.Web.Api.Cards.Checklists;

internal sealed class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        // Write 
        CreateMap<ChecklistCreationDto, ChecklistCreation>();
        CreateMap<ChecklistEditionDto, ChecklistEdition>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        CreateMap<ChecklistItemCreationDto, ChecklistItemCreation>();
        CreateMap<ChecklistItemEditionDto, ChecklistItemEdition>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        // Read
        CreateMap<Checklist, ChecklistDto>();

        CreateMap<ChecklistItem, ChecklistItemDto>();
    }
}
