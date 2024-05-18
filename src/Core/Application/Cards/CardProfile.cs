using System.Linq;
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
        
        // Read
        CreateMap<Card, CardDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id)
            .ForMember(dest => dest.ListBoardId, opt => opt.MapFrom(src => src.ListBoard.Id))
            .ForMember(dest => dest.ChecklistItemsCount, opt => opt.MapFrom(src => src.Checklists.SelectMany(ch => ch.ChecklistItems).Count()))
            .ForMember(dest => dest.ChecklistItemsDoneCount, opt => opt.MapFrom(src => src.Checklists.SelectMany(ch => ch.ChecklistItems).Count(i => i.IsChecked)));

        CreateMap<Card, CardDetailDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id)
            .ForMember(dest => dest.ListBoardId, opt => opt.MapFrom(src => src.ListBoard.Id))
            .ForMember(dest => dest.ListBoardName, opt => opt.MapFrom(src => src.ListBoard.Name))
            .ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.ListBoard.Board.Id));
    }
}