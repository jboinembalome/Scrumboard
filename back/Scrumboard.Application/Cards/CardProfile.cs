using AutoMapper;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Application.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<CardInputBase, Card>()
            .ForMember(dest => dest.Assignees, opt => opt.MapFrom(src => src.AssigneeIds))
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.LabelIds))
            .IncludeAllDerived();
        
        CreateMap<CardCreation, Card>()
            .ConstructUsing(src => new Card(
                src.Name, 
                src.Description, 
                src.DueDate,
                src.Position,
                src.ListBoardId,
                src.AssigneeIds,
                src.LabelIds));
    }
}
