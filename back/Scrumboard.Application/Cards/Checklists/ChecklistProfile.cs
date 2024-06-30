using AutoMapper;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards.Checklists;

namespace Scrumboard.Application.Cards.Checklists;

internal sealed class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        // Write 
        CreateMap<ChecklistDto, Checklist>();
        CreateMap<ChecklistItemDto, ChecklistItem>();
        
        // Read
        CreateMap<Checklist, ChecklistDto>();

        CreateMap<ChecklistItem, ChecklistItemDto>();
    }
}
