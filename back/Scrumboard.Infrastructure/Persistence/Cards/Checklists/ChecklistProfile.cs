using AutoMapper;
using Scrumboard.Domain.Cards.Checklists;

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

internal sealed class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        // Write 
        CreateMap<Checklist, ChecklistDao>();
        CreateMap<ChecklistItem, ChecklistItemDao>();
        
        // Read
        CreateMap<ChecklistDao, Checklist>();
        CreateMap<ChecklistItemDao, ChecklistItem>();
    }
}
