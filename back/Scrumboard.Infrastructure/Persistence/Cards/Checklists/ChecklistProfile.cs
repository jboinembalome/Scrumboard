using AutoMapper;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Checklists;

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

internal sealed class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        // Write 
        CreateMap<ChecklistCreation, ChecklistDao>();
        CreateMap<ChecklistEdition, ChecklistDao>();
        
        CreateMap<ChecklistItemCreation, ChecklistItemDao>();
        CreateMap<ChecklistItemEdition, ChecklistItemDao>();
        
        // Read
        CreateMap<ChecklistDao, Checklist>();
        
        CreateMap<ChecklistItemDao, ChecklistItem>();
    }
}
