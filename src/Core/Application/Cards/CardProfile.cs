using AutoMapper;
using Scrumboard.Application.Cards.Commands.CreateCard;
using Scrumboard.Application.Cards.Commands.UpdateCard;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<CreateCardCommand, Card>();

        CreateMap<UpdateCardCommand, Card>();
    }
}