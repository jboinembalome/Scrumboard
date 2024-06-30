using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.Cards.Checklists;

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

internal sealed class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        // Write 
        CreateMap<Checklist, ChecklistDao>()
            .EqualityComparison((dest, src) => dest.Id == src.Id);
        CreateMap<ChecklistItem, ChecklistItemDao>()
            .EqualityComparison((dest, src) => dest.Id == src.Id);
        
        // Read
        CreateMap<ChecklistDao, Checklist>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        CreateMap<ChecklistItemDao, ChecklistItem>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}
