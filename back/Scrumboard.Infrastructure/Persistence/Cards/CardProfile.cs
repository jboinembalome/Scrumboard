using AutoMapper;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<CardInputBase, Card>()
            .ForMember(dest => dest.Assignees, opt => opt.MapFrom(src => src.AssigneeIds))
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.LabelIds))
            .IncludeAllDerived();
        
        CreateMap<CardCreation, Card>();
        CreateMap<CardEdition, Card>();
    }
}
