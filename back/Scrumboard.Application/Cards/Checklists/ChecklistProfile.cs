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
            .EqualityComparison((dest, src) => dest.Id == src.Id);
        CreateMap<ChecklistItemDto, ChecklistItem>()
            .EqualityComparison((dest, src) => dest.Id == src.Id);
        
        // Read
        CreateMap<Checklist, ChecklistDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);

        CreateMap<ChecklistItem, ChecklistItemDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}
