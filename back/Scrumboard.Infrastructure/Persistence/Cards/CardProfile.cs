using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<Card, CardDao>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        CreateMap<string, CardAssigneeDao>()
            .ConstructUsing(assigneeId => new CardAssigneeDao
            {
                AssigneeId = assigneeId
            });
        
        // Read
        CreateMap<CardDao, Card>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        CreateMap<CardAssigneeDao, string>()
            .ConstructUsing(cardAssigneeDao =>  cardAssigneeDao.AssigneeId);
    }
}
