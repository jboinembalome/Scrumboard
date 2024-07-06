using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards.Checklists;

namespace Scrumboard.Application.Cards.Checklists;

internal sealed class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        // Write 
        CreateMap<ChecklistDto, Checklist>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        CreateMap<ChecklistItemDto, ChecklistItem>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        // Read
        CreateMap<Checklist, ChecklistDto>();

        CreateMap<ChecklistItem, ChecklistItemDto>();
    }
}
