using AutoMapper;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<CardInputBase, CardDao>()
            .ForMember(dest => dest.Assignees, opt => opt.MapFrom(src => src.AssigneeIds))
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.LabelIds))
            .IncludeAllDerived();
        
        CreateMap<CardCreation, CardDao>();
        CreateMap<CardEdition, CardDao>();
        
        CreateMap<string, CardAssigneeDao>()
            .ConstructUsing(assigneeId => new CardAssigneeDao
            {
                AssigneeId = assigneeId
            });
        
        // Read
        CreateMap<CardDao, Card>()
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees))
            .ForMember(dest => dest.LabelIds, opt => opt.MapFrom(src => src.Labels));
        
        CreateMap<CardAssigneeDao, string>();
    }
}
