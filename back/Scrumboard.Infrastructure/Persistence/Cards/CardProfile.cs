using AutoMapper;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<Card, CardDao>();
        
        CreateMap<string, CardAssigneeDao>()
            .ConstructUsing(assigneeId => new CardAssigneeDao
            {
                AssigneeId = assigneeId
            });
        
        // Read
        CreateMap<CardDao, Card>();
        
        CreateMap<CardAssigneeDao, string>();
    }
}
