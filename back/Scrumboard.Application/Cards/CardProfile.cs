using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Cards.Commands.CreateCard;
using Scrumboard.Application.Cards.Commands.UpdateCard;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<CreateCardCommand, Card>();

        CreateMap<UpdateCardCommand, Card>();
        CreateMap<CardDto, Card>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        // Read
        CreateMap<Card, CardDto>()
            .ForMember(dest => dest.ListBoardId, opt => opt.MapFrom(src => src.ListBoardId))
            .ForMember(dest => dest.ChecklistItemsCount, opt => opt.MapFrom(src => src.Checklists.SelectMany(ch => ch.ChecklistItems).Count()))
            .ForMember(dest => dest.ChecklistItemsDoneCount, opt => opt.MapFrom(src => src.Checklists.SelectMany(ch => ch.ChecklistItems).Count(i => i.IsChecked)));

        CreateMap<Card, CardDetailDto>()
            .ForMember(dest => dest.ListBoardId, opt => opt.MapFrom(src => src.ListBoardId));
    }
}
