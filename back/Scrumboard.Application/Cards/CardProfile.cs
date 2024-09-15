using AutoMapper;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
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
