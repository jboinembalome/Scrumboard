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

        // Read
        CreateMap<CardDao, Card>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}
